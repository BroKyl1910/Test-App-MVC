onload = () => {
    //https://stackoverflow.com/questions/12346381/set-date-in-input-type-date
    //Set date picker to today
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    $('.test-due-date-select').val(today);
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
var newQuestionButton = $('.new-question-button');
var saveTestButton = $('.save-test-button');

// Variables to store and control creation of questions
var testTitle;
var testDueDate;
var testModuleID;
var questions = [];
var questionIndex = 0;

// Store all questions in memory then when save test is clicked, send test to server
$(newQuestionButton).on('click', () => {
    if (saveQuestion()) {
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
        saveQuestion();
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

function saveQuestion() {
    if (validateQuestion()) {
        var question = {
            questionText: $(questionTextInput).val(),
            correctAnswer: parseInt($('input[name=correct-answer-radio]:checked').val()),
            answerA: $(aInput).val(),
            answerB: $(bInput).val(),
            answerC: $(cInput).val()
        };

        questions[questionIndex] = question;
        console.log(question);
        console.log(questions);

        return true;
    }
}

function saveTest() {
    if (validTest()) {
        var test = {
            title: $(testTitleInput).val(),
            moduleID: $(testModuleSelect).val(),
            dueDate: $(testDueDateSelect).val(),
            questions: questions
        }
        console.log(test);
        console.log("Saving Test");
    }
}

function validTest() {
    if (questions.length == 0) {
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
    $(questionHeading).text('Question ' + (questionIndex + 1));
    $(questionTextInput).val('');
    $(aInput).val('');
    $(bInput).val('');
    $(cInput).val('');
    $(aRadio).prop("checked", true);
}

function isEmpty(el) {
    return !$.trim(el.val())
}