//Javascript script blocks
function workMagic() {
    var val1 = Number(document.getElementById('magNum6').value);
    var val2 = Number(document.getElementById('magNum7').value);

    var val3 = '';

    for (var i = 1; i <= 100; i++) {
        
        val3 += !(i % val1) && !(i % val2) ? "<span>FizzBuzz</span>" :
            !(i % val1) ? "<span>Fizz</span>" : !(i % val2) ? "<span>Buzz</span>" :
            i;
        if (i >= 100) {
            val3 += " ";
        } else {
            val3 += ", ";
        }
    }
    $('#displayMagic').html(val3)
};

function numberCalculator() {
    var val1 = Number(document.getElementById('magNum1').value);
    var val2 = Number(document.getElementById('magNum2').value);
    var val3 = Number(document.getElementById('magNum3').value);
    var val4 = Number(document.getElementById('magNum4').value);
    var val5 = Number(document.getElementById('magNum5').value);

    var lowestNum = Math.min(val1, val2, val3, val4, val5);
    var highestNum = Math.max(val1, val2, val3, val4, val5);
    var totalNum = val1 + val2 + val3 + val4 + val5;
    var averageNum = (totalNum / 5);
    var totalNum = val1 + val2 + val3 + val4 + val5;
    var productNum = val1 * val2 * val3 * val4 * val5;

    document.getElementById('displayLowest').innerHTML = lowestNum;
    document.getElementById('displayHighest').innerHTML = highestNum;
    document.getElementById('displayAverage').innerHTML = averageNum;
    document.getElementById('displaySum').innerHTML = totalNum;
    document.getElementById('displayProduct').innerHTML = productNum;

};

function findFactorial() {
    var factorialNum = Number(document.getElementById('findFact').value);
    var factorialPreText = "The factorial of your number is: ";

    function factorial(factorialNum) {
        // If num is 0, its factorial is 1.
        // Else, call the recursive procedure
        if (factorialNum == 0) {
            return 1;
        } else {
            return (factorialNum * factorial(factorialNum - 1));
        }
    }

    //Parse input to an Integer
    factorialNum = parseInt(factorialNum);

    //If we did not get a number or it is less than 0
    //return an error message
    if (typeof factorialNum === 'number' && factorialNum >= 0) {
        document.getElementById('displayFactorial').innerHTML = factorialPreText + factorial(factorialNum);
    } else {
        alert('You must enter numbers greater than 0');
    }
}

function isPalindrome(str) {
    var passedTest = 'Your word is a palindrome!';
    var failedTest = 'Your word is NOT a palindrome!';
    var teststring = document.getElementById('palindromeTest').value;

    function IsPalindromeIteration(str) {
        var len = str.length, i = 0, result = true;
        if (len <= 1) return true;
        while (i != len - i - 1) {
            var start = str.charAt(i),
            end = str.charAt(len - i - 1);
            if (start != end) {
                return false;
            }
            i++;
        }
        return result;
    }

    var res = IsPalindromeIteration(teststring);
    if (res == true) {
        document.getElementById('palindromeResults').innerHTML = passedTest;
    } else {
        document.getElementById('palindromeResults').innerHTML = failedTest;
    }

}
//End javascript script blocks

//This clears out the content of each of the inputs in the modal popups and also clears out the results 
//divs with the class of jsClearOnExit
$('body').on('hidden.bs.modal', '.modal', function (event) {
    $(event.target).find('input').each(function (index, element) {
        $(element).val('')
    })
    $(event.target).find('.jsClearOnExit').each(function (index, HTMLDivElement) {
        $(HTMLDivElement).text('')
    })
});