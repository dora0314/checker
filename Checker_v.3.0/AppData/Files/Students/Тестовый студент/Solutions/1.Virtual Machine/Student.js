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