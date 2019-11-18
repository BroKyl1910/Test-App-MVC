//jQuery components
var questionHeading = $('.question-heading');
var questionText = $('.question-text');
var aText = $('.a-text');
var aRadio = $('.a-radio');
var bText = $('.b-text');
var cText = $('.c-text');
var nextQuestionButton = $('.next-question-button');
var saveTestButton = $('.finish-test-button');

// Variables to store and control creation of questions
var testID;
var questions = [];
var answers = [];
var questionIndex = 0;

onload = () => {
    testID = $('#testID').val();
    $.ajax({
        url: '/Tests/Questions',
        method: 'Get',
        data: {
            testID: testID
        },
        success: (_questions) => {
            questions = _questions;
            displayQuestion();
        }
    });
}

function displayQuestion() {
    var question = questions[questionIndex];
    $(questionHeading).text('Question ' + (questionIndex + 1));
    $(questionText).text(question.questionText);
    $(aText).text(question.answer1);
    $(bText).text(question.answer2);
    $(cText).text(question.answer3);
    $(aRadio).prop('checked', 'checked');

    if (questionIndex == questions.length - 1) {
        $(nextQuestionButton).fadeOut();
    }
}


// Store all questions in memory then when save test is clicked, send test to server
$(nextQuestionButton).on('click', () => {
    saveAnswer();
    questionIndex++;
    displayQuestion();

});

$(saveTestButton).on('click', () => {
    //If current question is blank, just save previous questions in case user doesn't fully understand UI
    saveAnswer();
    saveTest();

});

function saveAnswer() {
    var answer = parseInt($('input[name=correct-answer-radio]:checked').val());

    answers[questionIndex] = answer;
}

function saveTest() {
    console.log("Saving Test");

    $.ajax({
        url: '/Tests/Take',
        method: 'POST',
        data: {
            testID,
            answers
        },
        success: () => {
            window.location.replace('/Tests/Index');
        }

    });

}