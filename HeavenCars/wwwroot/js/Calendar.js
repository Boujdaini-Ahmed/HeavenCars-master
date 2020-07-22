//<script>
//    //$(document).ready(function () {
//    //        var events = [];
//    //        var selectedEvent = null;
//    //        FetchEventAndRenderCalendar();
//    //        function FetchEventAndRenderCalendar() {
//    //    events = [];
//    //            $.ajax({
//    //    type: "GET",
//    //                url: "/home/GetEvents",
//    //                success: function (data) {
//    //    $.each(data, function (i, v) {
//    //        events.push({
//    //            eventID: v.EventID,
//    //            title: v.Subject,
//    //            description: v.Description,
//    //            start: moment(v.Start),
//    //            end: v.End != null ? moment(v.End) : null,
//    //            color: v.ThemeColor,
//    //            allDay: v.IsFullDay
//    //        });
//    //    })

//    //                    GenerateCalender(events);
//    //                },
//    //                error: function (error) {
//    //    alert('failed');
//    //                }
//    //            })
//    //        }

//    //        function GenerateCalender(events) {
//    //    $('#calender').fullCalendar('destroy');
//    //            $('#calender').fullCalendar({
//    //    contentHeight: 400,
//    //                defaultDate: new Date(),
//    //                timeFormat: 'h(:mm)a',
//    //                header: {
//    //    left: 'prev,next today',
//    //                    center: 'title',
//    //                    right: 'month,basicWeek,basicDay,agenda'
//    //                },
//    //                eventLimit: true,
//    //                eventColor: '#378006',
//    //                events: events
//    //})
//    //}
//    //})
//    //</script>
//             }      