
$("#btn-search-employee").click(function () {
    let inputSearchEmployee = $("#input-search-employee").val();
    let URLAction = '/TrainingPrograms/RenderEmployee';
    $.ajax({
        url: URLAction,
        type: 'POST',
        data: { Searching: inputSearchEmployee, CheckedEmployeeIds: $("#employeeIdsString").val() },
        success: function (response) {
            $("#employeeTable").html(response);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
})

$("#submit").click(function (e) {
    e.preventDefault();
    let URLAction = '/TrainingPrograms/Create';
    let name = $("#name-input").val();
    if (name.trim().length == 0) {
        $("#validate-name").text("Name cannot be empty");
        return;
    } else {
        $("#validate-name").text("");
    }

    let description = $("#description-input").val();
    if (description.trim().length == 0) {
        $("#validate-description").text("Description cannot be empty");
        return;
    } else {
        $("#validate-description").text("");
    }

    let teacher = $("#teacher-input").val();
    if (teacher.trim().length == 0) {
        $("#validate-teacher").text("Teacher cannot be empty");
        return;
    } else {
        $("#validate-teacher").text("");
    }
    let createdAt = $("#createdAt-input").val();

    let employeeIds = JSON.parse($("#employeeIdsString").val());

    if (employeeIds.length == 0) {
        $("#error-message").text("Please select at least 1 employee");
        return;
    } else {
        $("#error-message").text("");
    }
    console.log(createdAt)

    $.ajax({
        url: URLAction,
        type: 'POST',
        data: { Name: name, Description: description, CreatedAt: createdAt, Teacher: teacher, EmployeeIds: JSON.stringify(employeeIds) },
        success: function (response) {
            if (response.statusCode != 200) {
                $("#error-message").text(response.message);
            } else {
                window.location.href = response.redirectToUrl;
            }

        }
    });
})