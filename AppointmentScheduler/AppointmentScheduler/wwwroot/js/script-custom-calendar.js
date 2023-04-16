var routeURL = location.protocol + "//" + location.host;
$(document).ready(function () {
    $("#appointmentDate").kendoDateTimePicker({
        value: new Date(),
        dateInput: false,
        format: "MM/dd/yyyy HH:mm:ss"
    });

    InitializeCalendar();
});

function DateFormatter(inputDate){
    var date = new Date(inputDate);
    var formattedDate = date.getFullYear() + '-' +
        ('0' + (date.getMonth() + 1)).slice(-2) + '-' +
        ('0' + date.getDate()).slice(-2) + 'T' +
        ('0' + date.getHours()).slice(-2) + ':' +
        ('0' + date.getMinutes()).slice(-2) + ':' +
        ('0' + date.getSeconds()).slice(-2);

    console.log(formattedDate);
}

function InitializeCalendar() {
    try {

        var calendarEl = document.getElementById('calendar');
        if (calendarEl != null) {
            calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                headerToolbar: {
                    left: 'prev,next,today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
                selectable: true,
                editable: false,
                select: function (event) {
                    onShowModal(event, null);
                },
                eventDisplay: 'block',
                events: function (fetchInfo, successCallback, failureCallback) {
                    $.ajax({
                        url: routeURL + '/api/Appointment/GetCalendarData?managerId=' + $("#managerId").val(),
                        type: 'GET',
                        dataType: 'JSON',
                        success: function (response) {

                            var events = [];
                            if (response.status === 1) {
                                $.each(response.dataenum, function (i, data) {
                                    events.push({
                                        title: data.title,
                                        description: data.description,
                                        start: data.startDate,
                                        end: data.endDate,
                                        backgroundColor: data.isManagerApproved ? "#28a745" : "#dc3545",
                                        borderColor: "#162466",
                                        textColor: "white",
                                        id: data.id
                                    });

                                })
                                console.log(events)

                            }
                            try {
                                successCallback(events);
                            }
                            catch (err) {
                                console.log(err);
                            }
                        },
                        error: function (xhr) {
                            console.log("Error")
                            $.notify("Error", "error");
                        }
                    });
                },
                eventClick: function (info) {
                    getEventDetailsByEventId(info.event);
                }
            });
            calendar.render();
        }

    }
    catch (e) {
        alert(e);
    }

}



function onShowModal(obj, isEventDetail) {
    //console.log("Triggered")
    $("#appointmentInput").modal("show");
}
function onCloseModal(obj, isEventDetail) {
    //console.log("Triggered")
    $("#appointmentInput").modal("hide");
}

function onSubmitForm() {
    if (checkValidation()) {
        var requestData = {
            Id: parseInt($("#id").val()),
            Title: $("#title").val(),
            Description: $("#description").val(),
            StartDate: $("#appointmentDate").val(),
            Duration: $("#duration").val(),
            AssociateId: $("#associateId").val(),
            ManagerId: $("#managerId").val(),
        };
        console.log(requestData);
        console.log(routeURL + '/api/Appointment/SaveCalendarData');

        $.ajax({
            url: routeURL + '/api/Appointment/SaveCalendarData',
            type: 'POST',
            data: JSON.stringify(requestData),
            contentType: "application/json",
            success: function (response) {
                if (response.status === 1 || response.status === 2) {
                    calendar.refetchEvents();                    
                    console.log(response.message);

                    console.log("success")
                    $.notify(response.message, "success");
                    onCloseModal();
                }
                else {
                    console.log(response.message);
                    $.notify(response.message, "error");
                }
            },
            error: function (xhr) {
                console.log(xhr);
                $.notify("Error", "error");
            }
        });
    }
}
function checkValidation() {
    var isValid = true;
    if ($("#title").val() === undefined || $("#title").val() == "") {
        isValid = false;
        $("#title").addClass('error');
    }
    else {
        $("#title").removeClass('error');
    }

    if ($("#appointmentDate").val() === undefined || $("#appointmentDate").val() == "") {
        isValid = false;
        $("#appointmentDate").addClass('error');
    }
    else {
        $("#appointmentDate").removeClass('error');
    }
    return isValid;

}