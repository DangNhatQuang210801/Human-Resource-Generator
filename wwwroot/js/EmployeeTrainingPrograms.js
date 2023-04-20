$("#check-all").change(function () {
    let checkAll = $(".check-item-employee")
    checkAll.prop("checked", this.checked);
    let checkAll2 = $(".check-item-employee:checked")
    if(checkAll === null){
        $("#employeeIdsString").val(JSON.stringify([]));
    }
    let newEmployeeIds = [];
    Array.from(checkAll2).forEach(c => {
        newEmployeeIds.push(c.value);
    });
    $("#employeeIdsString").val(JSON.stringify(newEmployeeIds));
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