//jQuery components
var questionHeading = $('.question-heading');
var questionText = $('.question-text');
var aText = $('.a-text');
var aRadio = $('.a-radio');
var bText = $('.b-text');
var bRadio = $('.b-radio');
var cText = $('.c-text');
var cRadio = $('.c-radio');
var nextQuestionButton = $('.next-question-button');
var prevQuestionButton = $('.prev-question-button');
var saveTestButton = $('.finish-test-button');

// Variables to store and control creation of questions
var testID;
var questions = [];
var answers = [];
var questionIndex = 0;

onload = () => {
    testID = $('#testID').val();
    console.log(testID, "testID");
    $.ajax({
        url: '/Tests/Questions',
        method: 'Get',
        data: {
            testID: testID
        },
        success: (_questions) => {
            questions = _questions;
            getAnswers();

        }
    });
}

function getAnswers() {
    $.ajax({
        url: '/Tests/Answers',
        method: 'Get',
        data: {
            testID: testID
        },
        success: (_answers) => {
            answers = JSON.parse(_answers);
            displayQuestion();
        }
    });
}

function displayQuestion() {
    var question = questions[questionIndex];
    var answer = answers[questionIndex];
    console.log(questions[questionIndex]);
    console.log(answer);
    $(questionHeading).text('Question ' + (questionIndex + 1));
    $(questionText).text(question.questionText);
    $(aText).text(question.answer1);
    $(bText).text(question.answer2);
    $(cText).text(question.answer3);
    var radioButtons = [$(aRadio), $(bRadio), $(cRadio)];
    $(radioButtons[answer.UserAnswer]).prop('checked', 'checked');
    $('input[type=radio]').attr('disabled', true);

    var correctOutput = $('.correct-output');
    //Display correct/incorrect
    var letters = ['A', 'B', 'C'];
    if (question.correctAnswer == answer.UserAnswer) {
        $(correctOutput).removeClass('text-danger');
        $(correctOutput).addClass('text-success');
        $(correctOutput).text('Correct Answer: ' + letters[question.correctAnswer]);
    } else {
        $(correctOutput).addClass('text-danger');
        $(correctOutput).removeClass('text-success');
        $(correctOutput).text('Correct Answer: ' + letters[question.correctAnswer]);
    }

    //Display prev/next
    if (questionIndex == questions.length - 1) {
        $(nextQuestionButton).css('visibility', 'hidden');
        if (questions.length == 1) {
            $(prevQuestionButton).css('visibility', 'hidden');
        }
    } else if (questionIndex == 0) {
        $(prevQuestionButton).css('visibility', 'hidden');
    } else {
        $(nextQuestionButton).css('visibility', 'visible');
        $(prevQuestionButton).css('visibility', 'visible');
    }
}

$(nextQuestionButton).on('click', () => {
    questionIndex++;
    displayQuestion();

});

$(prevQuestionButton).on('click', () => {
    questionIndex--;
    displayQuestion();

});

$(saveTestButton).on('click', () => {
    window.location.replace('/Tests/Index');
});