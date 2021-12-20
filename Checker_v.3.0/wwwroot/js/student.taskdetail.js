$(document).ready(function () {
    $("#checkSolutionButton").click(CheckSolution);
});

function LoadSolution(inputId) {
    var input = document.getElementById(inputId);
    var formData = new FormData();
    formData.append("studentResultId", $(input).data("id"));
    formData.append("studentFile", input.files[0]);

    var request = new XMLHttpRequest();
    request.open("POST", "/Students/LoadSolution");
    request.onload = function (e) {
        var response = JSON.parse(e.target.response);

        if (response.success) {
            $("#solutionLoadedDateTime").html("........" + response.loadTime + " " + response.loadDate);
            $("#fileNoteText").html("решение загружено");
        } else {
            $("#fileNoteText").html(response.message);
        }
    };
    request.send(formData);
}

function CheckSolution() {
    $.post({
        url: "/Students/CheckSolution?studentResultId=" + StudentTaskResultId,
        success: function (data) {
            var passedTestsCount = 0;
            for (var i = 0; i < data.data.length; i++) {
                $(".testLabel[data-id='" + data.data[i].testId + "']").html(data.data[i].testTitle + "............" + data.data[i].state.title);
                if (data.data[i].state.title == "Пройден")
                    passedTestsCount++;
            }
            $("#passedTestsCountLabel").html(passedTestsCount);
        }
    });
}