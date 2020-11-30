


function onlyAlphabets(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 32)
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}

function onlyNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}



function w3_open() {
    document.getElementById("main").style.marginLeft = "20%";
    document.getElementById("mySidebar").style.width = "20%";
    document.getElementById("mySidebar").style.display = "block";
    document.getElementById("openNav").style.display = 'none';
    document.getElementById("closeNav").style.display = 'block';
}
function w3_close() {
    document.getElementById("closeNav").style.display = 'none';
    document.getElementById("main").style.marginLeft = "0%";
    document.getElementById("mySidebar").style.display = "none";
    document.getElementById("openNav").style.display = "inline-block";

}

function clearValue(attachment) {
    document.getElementById(attachment).value = null;
    alert("Are you sure you want to delete this ? Press Save Button to Confirm");
}

function ReceiverFileSelected(fileInputId, buttonId) {
    if ($("#" + fileInputId).val != "") {
        document.getElementById(buttonId).disabled = false;
    }
}

function showSpinnerAttachment() {

    $("#bodyIdAttachment").addClass("disabledbutton");

    let opts = {
        lines: 13,
        length: 28,
        width: 14,
        radius: 42,
        scale: 1,
        corners: 1,
        color: 'black',
        opacity: 0.25,
        rotate: 0,
        direction: 1,
        speed: 1,
        trail: 60,
        fps: 20,
        zIndex: 2e9,
        className: 'spinner',
        top: '50%',
        left: '50%',
        shadow: false,
        hwaccel: false,
        position: 'fixed',
    },
        target = document.getElementById('spinnerattachment'),
        spinner = new Spinner(opts).spin(target);

}


function showSpinnerViewTransaction() {

    $("#bodyIdViewTransaction").addClass("disabledbutton");
    let opts = {
        lines: 13,
        length: 28,
        width: 14,
        radius: 42,
        scale: 1,
        corners: 1,
        color: 'black',
        opacity: 0.25,
        rotate: 0,
        direction: 1,
        speed: 1,
        trail: 60,
        fps: 20,
        zIndex: 2e9,
        className: 'spinner',
        top: '50%',
        left: '50%',
        shadow: false,
        hwaccel: false,
        position: 'fixed',
    },
        target = document.getElementById('spinnerViewTransaction'),
        spinner = new Spinner(opts).spin(target);

}



function exitSpinnerViewTransaction() {
    $("#bodyIdViewTransaction").addClass("enablebutton");
    $("#bodyIdAttachment").addClass("enablebutton");
//    spinner.stop();
}








//    $(function () {
//        $("#ReceiverAddress").typeahead({
//            hint: true,
//            highlight: true,
//            minLength: 1,
//            source: function (request, response) {
//                $.ajax({
//                    url: '/Home/MakePayments1/',
//                    data: "{ 'prefix': '" + request + "'}",
//                    dataType: "json",
//                    type: "POST",
//                    contentType: "application/json; charset=utf-8",
//                    success: function (data) {
//                        items = [];
//                        map = {};
//                        $.each(data, function (i, item) {
//                            var id = item.val;
//                            var name = item.label;
//                            map[name] = { id: id, name: name };
//                            items.push(name);
//                        });
//                        response(items);
//                        $(".dropdown-menu").css("height", "auto");
//                    },
//                    error: function (response) {
//                        alert(response.responseText);
//                    },
//                    failure: function (response) {
//                        alert(response.responseText);
//                    }
//                });
//            },
//            updater: function (item) {
//                $('#hfCustomer').val(map[item].id);
//                return item;
//            }
//        });
//});
