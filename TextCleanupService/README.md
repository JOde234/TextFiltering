# TextCleanupService

## Project Description

An application with the associated unit tests that can take multiple text filters and apply them
on any given string.

### The processing
* Read from a txt file
* Create and apply the following 3 filters:
  * filter out all the words that contains a vowel in the middle of the word – the centre 1 or 2 letters
("clean" middle is ‘e’, "what" middle is ‘ha’, "currently" middle is ‘e’ and will be filtered, "the", "rather"
will not be)
  * filter out words that have length less than 3
  * filter out words that contains the letter ‘t’
* After all filters have completed - display the resulted text in the console.