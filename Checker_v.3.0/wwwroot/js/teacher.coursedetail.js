$(document).ready(function () {
    $(".editTaskButton").on("click", function (e) {
        EditTask(e.target);
    });

    $("#createTaskButton").on("click", function (e) {
        CreateTask();
    });
});

function EditTask(target) {
    var id = $(target).data("id");

    var darkLayer = document.createElement('div'); // слой затемнения
    darkLayer.id = 'shadow'; // id чтобы подхватить стиль
    document.body.appendChild(darkLayer); // включаем затемнение

    var formWindow = document.getElementById('popupWin'); // находим наше "окно"
    $.get({
        url: "/Teachers/EditTask?taskId=" + id,
        success: function (data) {
            $(formWindow).html(data);
        }
    });

    formWindow.style.display = 'block'; // "включаем" его

    darkLayer.onclick = function () {  // при клике на слой затемнения все исчезнет
        darkLayer.parentNode.removeChild(darkLayer); // удаляем затемнение
        formWindow.style.display = 'none'; // делаем окно невидимым
        return false;
    };
}

function CreateTask() {
    var darkLayer = document.createElement('div'); // слой затемнения
    darkLayer.id = 'shadow'; // id чтобы подхватить стиль
    document.body.appendChild(darkLayer); // включаем затемнение

    var formWindow = document.getElementById('popupWin'); // находим наше "окно"
    $.get({
        url: "/Teachers/CreateTask?courseId=" + CourseId,
        success: function (data) {
            $(formWindow).html(data);
        }
    });

    formWindow.style.display = 'block'; // "включаем" его

    darkLayer.onclick = function () {  // при клике на слой затемнения все исчезнет
        darkLayer.parentNode.removeChild(darkLayer); // удаляем затемнение
        formWindow.style.display = 'none'; // делаем окно невидимым
        return false;
    };
}