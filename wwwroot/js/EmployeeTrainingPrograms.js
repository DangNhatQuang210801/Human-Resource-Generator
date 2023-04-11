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