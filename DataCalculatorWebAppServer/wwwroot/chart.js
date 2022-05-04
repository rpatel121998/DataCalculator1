var state = 0;

var chart = null;

window.setup = (id, config) => {

    var ctx = document.getElementById(id).getContext('2d');

    chart = new Chart(ctx, config);

}

