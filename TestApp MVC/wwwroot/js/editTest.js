// Variables to store and control creation of questions
var testTitle;
var testDueDate;
var testModuleID;
var testID;
var test = {};
var questionIndex = 0;

onload = () => {
    testID = $('#test-id').val();
    $.ajax({
        url: '/Tests/MemoQuestions',
        method: 'get',
        data: {
            testID: testID
        },
        success: (_test) => {
            console.log(_test);
            test = _test;
            $('.test-due-date-select').val(test.dueDate.replace(new RegExp('/', 'g'), '-'));
            clearQuestion();
        }
    });

    ////https://stackoverflow.com/questions/12346381/set-date-in-input-type-date
    ////Set date picker to today
    //var now = new Date();
    //var day = ("0" + now.getDate()).slice(-2);
    //var month = ("0" + (now.getMonth() + 1)).slice(-2);
    //var today = now.getFullYear() + "-" + (month) + "-" + (day);
}

//jQuery components
var testTitleInput = $('.test-title-input');
var testModuleSelect = $('.test-module-select');
var testDueDateSelect = $('.test-due-date-select');
var questionHeading = $('.question-heading');
var questionTextInput = $('.question-text-input');
var aRadio = $('.a-radio');
var aInput = $('.a-input');
var bRadio = $('.b-radio');
var bInput = $('.b-input');
var cRadio = $('.c-radio');
var cInput = $('.c-input');
var nextQuestionButton = $('.new-question-button');
var saveTestButton = $('.save-test-button');


// Store all questions in memory then when save test is clicked, send test to server
$(nextQuestionButton).on('click', () => {
    if (saveAnswer()) {
        questionIndex++;
        clearQuestion();
    }
});

$(saveTestButton).on('click', () => {
    //If current question is blank, just save previous questions in case user doesn't fully understand UI
    if (questionEmpty()) {
        saveTest();
    } else {
        //Question not empty so validate question then save all questions as test
        saveAnswer();
        saveTest();
    }

});

// Determine if question is empty
function questionEmpty() {
    return isEmpty($(questionTextInput)) && isEmpty($(aInput)) && isEmpty($(bInput)) && isEmpty($(cInput));
}

function validateQuestion() {
    //Sequence of null checks
    if (isEmpty($(questionTextInput))) {
        $('.question-error').text('Please enter a question');
        return false;
    }

    if (isEmpty($(aInput)) || isEmpty($(bInput)) || isEmpty($(cInput))) {
        $('.question-error').text('Please enter an answer for all 3 options');
        return false;
    }


    $('.question-error').text('');
    return true;
}

function saveAnswer() {
    if (validateQuestion()) {
        var question = {
            QuestionID: test.questions[questionIndex].questionID,
            QuestionText: $(questionTextInput).val(),
            CorrectAnswer: parseInt($('input[name=correct-answer-radio]:checked').val()),
            Answer1: $(aInput).val(),
            Answer2: $(bInput).val(),
            Answer3: $(cInput).val()
        };

        test.questions[questionIndex] = question;
        console.log(test.questions[questionIndex]);
        console.log(test.questions);

        return true;
    }
}

function saveTest() {
    if (validTest()) {
        test.title = $(testTitleInput).val();
        test.moduleID = $(testModuleSelect).val();
        test.dueDate = $(testDueDateSelect).val();


        console.log(test);
        console.log("Saving Test");
        test.question = test.questions;
        $.ajax({
            url: '/Tests/Edit',
            method: 'POST',
            data: {
                test: test
            },
            success: (res) => {
                window.location.replace('/Tests/Index');
            }

        });
    }
}

function validTest() {
    if (test.questions.length == 0) {
        $('.test-error').text('Please create at least 1 question');
        return false;
    }

    if (isEmpty($(testTitleInput))) {
        $('.test-error').text('Please enter a title');
        return false;
    }

    if ($(testModuleSelect).val() == "-1") {
        $('.test-error').text('Please select a module');
        return false;
    }

    // Due date can't be before today
    if (new Date($(testDueDateSelect).val()).setHours(0, 0, 0, 0) <= new Date().setHours(0, 0, 0, 0)) {
        $('.test-error').text('Please select due date after today');
        return false;
    }

    $('.test-error').text('');
    return true;

}

// Clears currently filled form and display new question index
function clearQuestion() {
    if (questionIndex == test.questions.length - 1) {
        $(nextQuestionButton).css('visibility', 'hidden');
    }

    $(questionHeading).text('Question ' + (questionIndex + 1));
    $(questionTextInput).val(test.questions[questionIndex].questionText);
    $(aInput).val(test.questions[questionIndex].answer1);
    $(bInput).val(test.questions[questionIndex].answer2);
    $(cInput).val(test.questions[questionIndex].answer3);

    var radioButtons = [$(aRadio), $(bRadio), $(cRadio)];
    $(radioButtons[test.questions[questionIndex].correctAnswer]).prop('checked', 'checked');

}

function isEmpty(el) {
    return !$.trim(el.val())
}