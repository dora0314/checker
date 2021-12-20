$(document).ready(function () {
    $("#createTestButton").on("click", function (e) {
        CreateTest(e.target);
    });

    $("#editTaskButton").on("click", function (e) {
        EditTask(e.target);
    });

    $(".editTestButton").on("click", function (e) {
        EditTest(e.target);
    });
});

function CreateTest(target) {
    var darkLayer = document.createElement('div'); // слой затемнения
    darkLayer.id = 'shadow'; // id чтобы подхватить стиль
    document.body.appendChild(darkLayer); // включаем затемнение

    var taskId = $(target).data("id");

    var modalWin = document.getElementById('popupWin'); // находим наше "окно"
    $.get("/Teachers/CreateTest?taskId=" + taskId, { layout: null }, function (data) {
        $("#popupWin").html(data);
    });
    
    modalWin.style.display = 'block'; // "включаем" его

    darkLayer.onclick = function () {  // при клике на слой затемнения все исчезнет
        darkLayer.parentNode.removeChild(darkLayer); // удаляем затемнение
        modalWin.style.display = 'none'; // делаем окно невидимым
        return false;
    };
}

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

function EditTest(target) {
    var id = $(target).data("id");

    var darkLayer = document.createElement('div'); // слой затемнения
    darkLayer.id = 'shadow'; // id чтобы подхватить стиль
    document.body.appendChild(darkLayer); // включаем затемнение

    var formWindow = document.getElementById('popupWin'); // находим наше "окно"
    $.get({
        url: "/Teachers/EditTest?testId=" + id,
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