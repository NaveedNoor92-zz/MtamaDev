using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mtama.Data;
using Mtama.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mtama.Scheduler
{
    public class ScheduleTask : ScheduledProcessor
    {
        ApplicationDbContext _context = null;
        EthService _es = null;
        private readonly IServiceScopeFactory _scopeFactory;

        public ScheduleTask(IServiceScopeFactory serviceScopeFactory, ILogger<ScheduleTask> logger) : base(serviceScopeFactory)
        {
            _scopeFactory = serviceScopeFactory;
            _logger = logger;
            //_context = context;
        }

        protected override string Schedule => ConfigurationManager.GetAppSetting("Schedule");

        public override Task ProcessInScope(IServiceProvider serviceProvider)
        {
            MakeTransfers();
            
            return Task.CompletedTask;
        }
                
        private async void MakeTransfers()
        {
            Log("START: Executing Mtama Background Service");
            using (var scope = _scopeFactory.CreateScope())
            {
                _es = new EthService();
                try
                {
                    _es.Init();
                    Log("Mtama Contract Initialized");
                }
                catch (Exception ex)
                {
                    Log("Error contacting Mtama contract", true);
                    Log(ex.Message + " - " + ex.StackTrace, true);                    
                }

                try
                {
                    var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var verifiedPayments = _context.Payments.Where(p => p.TxStatus == Common.TxStatusVerified);
                    var pendingPayments = _context.Payments.Where(p => p.TxStatus == Common.TxStatusPending);

                    int escrowTime = 30;

                    try
                    {
                        escrowTime = int.Parse(ConfigurationManager.GetAppSetting("EscrowTime"));
                    }
                    catch {}
                    
                    foreach (var payment in verifiedPayments)
                    {
                        var txHash2 = await Execute(payment, false);
                        if (txHash2 == "0") continue;

                        payment.TxHash2 = txHash2;
                        payment.TxStatus = Common.TxStatusTransferred;
                        _context.Payments.Update(payment);
                    }

                    foreach (var payment in pendingPayments)
                    {
                        if(payment.TimeStamp.AddDays(escrowTime) < DateTime.Now)
                        {
                            var txHash2 = await Execute(payment, true);
                            if (txHash2 == "0") continue;

                            payment.TxHash2 = txHash2;
                            payment.TxStatus = Common.TxStatusRefunded;
                            _context.Payments.Update(payment);
                        }                        
                    }
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log("Error getting payments from DB", true);
                    Log(ex.Message + " - " + ex.StackTrace, true);
                }

            }

            Log("END: Executing Mtama Background Service");
        }
        private async Task<string> Execute(PaymentModel payment, bool isRefund)
        {
            Log("Executing Verified Payment - TxGuid: " + payment.TxGuid);
            try
            {
                return await _es.Execute(payment.TxGuid, isRefund);
            }
            catch (Exception ex)
            {
                Log("Error executing BC transaction: " + payment.TxGuid + " - " + ex.Message + ex.StackTrace);
                return "0";
            }
        }
        
        private void Log(string msg, bool error = false)
        {
            Console.WriteLine(msg);
            if (error) _logger.LogError(msg);
            else _logger.LogInformation(msg);
        }
    }
}
