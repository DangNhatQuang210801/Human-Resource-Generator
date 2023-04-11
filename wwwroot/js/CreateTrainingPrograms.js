$("#check-all").change(function () {
    $(".check-item-employee").prop("checked", this.checked);
})

$(".check-item-employee").change(function () {
    let totalChecked = $(".check-item-employee:checked").length;
    let totalRecord = $("#totalEmployee").val();
    if (totalChecked == totalRecord) {
        $("#check-all").prop("checked", this.checked);
    } else {
        $("#check-all").prop("checked", false);
    }

    let newEmployeeIds = [];
    if ($("#employeeIdsString").val() != "") {
        newEmployeeIds = JSON.parse($("#employeeIdsString").val());
        newEmployeeIds.forEach((e, i) => {
            newEmployeeIds[i] = parseInt(e);
        })
    }
    if (newEmployeeIds.includes(parseInt($(this).val()))) {
        if (!this.checked) {
            newEmployeeIds = newEmployeeIds.filter(item => item !== parseInt($(this).val()))
        }
    } else {
        if (this.checked) {
            newEmployeeIds.push($(this).val())
        }
    }
    $("#employeeIdsString").val(JSON.stringify(newEmployeeIds));
})

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

    let employeeIds = JSON.parse($("#employeeIdsString").val());

    if (employeeIds.length == 0) {
        $("#error-message").text("Please select at least 1 employee");
        return;
    } else {
        $("#error-message").text("");
    }

    $.ajax({
        url: URLAction,
        type: 'POST',
        data: { Name: name, Description: description, Teacher: teacher, EmployeeIds: JSON.stringify(employeeIds) },
        success: function (response) {
            if (response.statusCode != 200) {
                $("#error-message").text(response.message);
            } else {
                window.location.href = response.redirectToUrl;
            }

        }
    });
})