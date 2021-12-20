// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function Run(n) {
    var one = 1;
    var two = 1;

    for (var i = 2; i < n; i++) {
        var curr = two;
        two = one + two;
        one = curr;
    }

    return two;
}

var result = false;

function Test() {
    result = Run(5) == 5;
}

Test();