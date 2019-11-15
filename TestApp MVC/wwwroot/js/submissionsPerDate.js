var testContainers = $('.test-container');
var testIds = [];
for (var container of testContainers) {
    testIds.push(parseInt($(container).attr('data-id')));
}

for (var id of testIds) {
    $.ajax({
        method: "get",
        url: "/Graphs/TestSubmissionsPerDate",
        data: {
            testID: id
        },
        async: false,
        success: (data) => {
            data = JSON.parse(data);

            var xTickValues = data.xDates.slice();
            var yTickValues = data.ySubmissions.slice();

            data.ySubmissions.unshift('Submissions');
            data.xDates.unshift('x');

            $('#num-submissions_' + id).text('Number of submissions:' + data.totalSubmissions);
            var chart = c3.generate({
                bindto: '#test_' + id,
                data: {
                    x: 'x',
                    columns: [
                        data.ySubmissions,
                        data.xDates
                    ],
                    type: 'bar'
                },
                bar: {
                    width: 15
                },
                axis: {
                    x: {
                        type: 'timeseries',
                        tick: {
                            values: xTickValues,
                            centered: true,
                            fit: false,
                            format: "%e %b %y",
                            rotate: 90
                        }
                    },
                    y: {
                        tick: {
                            values: yTickValues
                        }
                    }
                }
            });
        }
    });
}