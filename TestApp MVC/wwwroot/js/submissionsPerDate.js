var testContainers = $('.test-container');
var testIds = [];
for (var container of testContainers) {
    testIds.push(parseInt($(container).attr('data-id')));
}

for (var id of testIds) {
    console.log('#test_' + id);
    var chart = c3.generate({
        bindto: '#test_' + id,
        data: {
            columns: [
                ['data1', 30, 200, 100, 400, 150, 250]
            ],
            type: 'bar'
        }
    });
}