// declare map outside of initialize function so that drawPoints() can use it
var map;
var coordString = "";

// array to store markers that user has drawn
var markers_track = [];

// called when page is loaded
window.onload = function () {
    // arbitrary point
    //var myLatLng = new google.maps.LatLng(31.451881188002115, 74.30786712976669);


    var myLatLng = new google.maps.LatLng(-0.0236, 37.9062);
    
    var centermarker, poly; 

    // options to init map with, again arbitrary
    var myOptions = {
        zoom: 10,
        center: myLatLng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    // get our map object
    map = new google.maps.Map(document.getElementById("mymaps"), myOptions);

   

    // array to store markers that user has drawn
    var markers = [];

    if (isNaN(areaMarkers) && typeof areaMarkers !== 'undefined') {
        for (i = 0; i < areaMarkers.length; i++) {
            var data = areaMarkers[i];
            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.title
            });
            markers.push(marker);
            markers_track.push(marker);
        }
    }

    drawPoints(markers);
    markers = []; 

    document.getElementById("resetbtn").addEventListener("click", resetFunc);

    // add event listener to the map to get the click events and draw a marker
    google.maps.event.addListener(map, 'click', function (e) {
            var marker = new google.maps.Marker();
            marker.setPosition(e.latLng);
            marker.setMap(map);
            marker.setVisible(true);

            // push it to the markers array
            markers.push(marker);
            markers_track.push(marker);

            // add an event listener to each marker so that we can draw a polygon
            // once user clicks on on of the markers in the markers array, assuming
            // that they are ready to join up the polygon and have it drawn
            google.maps.event.addListener(marker, 'click', function (e) {
                drawPoints(markers);

                // empty the markers array so that the user can draw a new one, without
                // them all joining up. this is perphaps where you would want to push
                // the markers array to a database, storing the points as latitude/longitude
                // values so that they can be retrieved, put into an array, and drawn
                // as a polygon again. 
                markers = [];
            });
    });
};

function resetFunc() {
    for (var i = 0; i < markers_track.length; i++) {
        markers_track[i].setMap(null);
    }
    document.getElementById("profileViewModel_Field_Coords").value = "[]";
    document.getElementById("profileViewModel_mapCoords").value = "[]";
    document.getElementById("profileViewModel_Field_pin").value = "[]";
    //poly.setPath(null);
}

function drawPoints(markers) {
    poly = new google.maps.Polygon;
    markers_track.push(poly);
    var bounds = new google.maps.LatLngBounds(); 

    var points = [];

    for (var i = 0; i < markers.length; i++) {
        points.push(markers[i].getPosition());
    }

    for (var i = 0; i < points.length; i++) {
        bounds.extend(points[i]);
    }

    poly.setMap(map);
    poly.setPath(points);
    poly.setVisible(true);

    //setting the center pin
    centermarker = new google.maps.Marker();
    markers_track.push(centermarker);

    centermarker.setPosition(bounds.getCenter());
    centermarker.setMap(map);
    centermarker.setVisible(true);


    if (centermarker.getPosition().lat().toString() !== "0" && centermarker.getPosition().lng().toString() !== "-180") {
        var coordString1 = "lat: "+ centermarker.getPosition().lat().toString() + ", " +
            "lng: " + centermarker.getPosition().lng().toString();

        document.getElementById("profileViewModel_Field_pin").value = coordString1;
        var PanToPlot = new google.maps.LatLng(centermarker.getPosition().lat().toString(), centermarker.getPosition().lng().toString());
        map.setZoom(15);
        map.panTo(PanToPlot);
    } 
    else {
        document.getElementById("profileViewModel_Field_pin").value = "[]";
    }

    drawpointcalled = true;
    toCoordString(markers);
}

function toCoordString(markersArray) {

    coordString += "[";
    for (var i = 0; i < markersArray.length; i++) {
        coordString += "{'lat': " + "'" + markersArray[i].getPosition().lat().toString() + "',";
        coordString += "'lng': " + "'" + markersArray[i].getPosition().lng().toString() + "',},";
    }
    coordString += "]";

    document.getElementById("profileViewModel_mapCoords").value = coordString;
    document.getElementById("profileViewModel_Field_Coords").value = coordString;
    coordString = "";
}


