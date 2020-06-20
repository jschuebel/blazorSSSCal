window.appstuff = {
    jsShowPop : function() {
        DotNet.invokeMethodAsync('SSSCalBlazor.Pages', 'ShowPop')
            .then(data => {
                
                console.log('made it back');
            });
    }
}

window.getFolderWidth = function (tFolder) {
    return tFolder.offsetWidth;
}

window.playmovie = function (element, path) {
    var vpID = document.getElementById("vpID");

    vpID.style.display = "block";

    const y = vpID.getBoundingClientRect().top + window.scrollY;
    window.scroll({
        top: y,
        behavior: 'smooth'
    });

    element.src = path;
    element.play();
}

window.stopmovie = function (element) {
    var vpID = document.getElementById("vpID");
    vpID.style.display = "none";
    element.stop();
}


window.openpic = function (element, path) {
    var nwin = window.open("./PicsPage.html");
    nwin.opener.pth2 = path;

}