var testIDs = [];
onload = () => {
    drawGraph();
}

$('#module-select').on('change', () => {
    drawGraph();
    $('.test-container').slideUp();

});

function drawGraph() {
    $.ajax({
        method: "get",
        url: "/Graphs/AveragePerModule",
        data: {
            moduleID: $('#module-select').val()
        },
        async: false,
        success: (data) => {
            data = JSON.parse(data);
            testIDs = data.testIDs;
            if (data.yAverages.length == 0) {
                $('.average-graph').css('visibility', 'hidden');
                return;
            }

            var xTickValues = data.xTests.slice();
            var yTickValues = data.yAverages.slice();

            data.yAverages.unshift('Average');
            data.xTests.unshift('x');

            var chart = c3.generate({
                bindto: '.average-graph',
                padding: {
                    top: 30,
                    right: 30,
                    bottom: 30,
                    left: 30,
                },
                data: {
                    x: 'x',
                    columns: [
                        data.yAverages,
                        data.xTests
                    ],
                    type: 'bar',
                    onclick: (d, e) => {
                        showTestResults(d.index);
                    }
                },
                bar: {
                    width: 15
                },
                axis: {
                    x: {
                        type: 'category',
                        categories: xTickValues,
                        tick: {
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

function showTestResults(index) {
    var testID = testIDs[index];
    $.ajax({
        url: '/Graphs/TestResults',
        method: 'get',
        data: {
            testID: testID
        },
        success: (data) => {
            data = JSON.parse(data);

            $('#test-title').text(data[0].title);
            $('.results-ul').empty();
            for (var res of data) {
                var name = res.name;
                var result = res.result;

                var li = '<li class="results-li"><span class="result-name">' + name + '</span><span class="result-percentage">' + result + '%</span></li>'
                $('.results-ul').append(li);
            }
            $('.test-container').slideDown();
        }
    });
}