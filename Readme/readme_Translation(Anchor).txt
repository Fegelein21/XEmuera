
================== How to create proper translation of era Games using Anchor ===================

Current version: 1.24

# Changes from version 1.24
- Added TR_NAME commands which returns

# Changes from version 1.19
- STR_TR.csv now translating properly
- Added support for translating characters using TRCHARA file, see 2
- Multiple bug fixes and functions added since v1.10

# Changes from version 1.10
- Added comment support

# Changes from version 1.09
- Most translations will now work without putting it in ALL_TR.csv
- This should be the final version of where you need to place the translation for it to work.

================================ 1 - CSV Translation System (TR) ================================

You can now translate variable names without doing mass replaces for CSV.
To translate the printing of a variable you need to:

1) Create the file XXXXX_TR.csv where XXXXX is where the variable is named like Base_TR.csv or Talent_TR.csv if it doesn't already exist.
2) Put originalName,newName alone on a line.

Everything after ; is a comment.

; Example of lines in TR files.
Male,Trap; This part is a comment.
LargeMirror,Large mirror



Only these files can have a XXXX_TR.csv version.
{"Abl", "Talent", "Exp", "Mark", "Palam", "Item","Train", "Base","Source", "Ex", "EQUIP", "TEQUIP", "Flag", "TFLAG", "Cflag", "Tcvar", "CSTR", "Stain","Cdflag1", "Cdflag2", "Str", "TSTR","SaveStr", "GLOBAL", "GLOBALS", "ALL"}

-- In case the original name stills shows --

The name is sometimes written directly in a print, to mass replace it without touching the variable please use this regular expression: (?<=Print.*|SET_PICTURE.*)(?<!:)\b(XXX)\b(?!"). It is possible that it will miss rare occurrence but it will not touch variables.
That said, it's preferable to do things by hand still.


-- In the case it's still not working. Create ALL_TR.csv if it's not already --
(Warning: This feature may be phased out in the future)

It replace directly what is between %% in a PRINTFORM.

Put originalName,newName alone on a line.  (Same things as the others _TR for now)
Male,Trap; This part is a comment
LargeMirror,Large mirror

========================== 2 - Character Translation System (TRCHARA) ===========================

In order to translate character files and their contents, you need to use a system different from the above.

1) Create a file named TRCHARA.csv inside the root CSV one or any of its sub-folders, this will contain the translated content.
	1b) As long as the filename starts with 'TRCHARA' (all caps) and ends with '.csv', it may have any name.
		"TRCHARA(キミキス).csv" for example is a valid TRCHARA filename.
	1a) For better organization, you can create multiple TRCHARA.csv files, each having different character translation data.
		Better yet if you name each differently

2) Add translation data for each character in the TRCHARA file, as follows:
	2a) First start with "NO,XXX" where XXX is the character number on the character csv file.
		It'll accept "番号,XXX" as well, if you prefer to copy&paste from the original file.
	2b) From here on, all steps are optional, add just what you wish to translate, each on a single line of text:
		"NAME,Full Translated Name," or "名前,Full Translated Name,"
	2c) "CALLNAME,Nickname," or "呼び名,Nickname,". Remember, there's an "," at the end.
	2d) "NICKNAME,Nickname," or "あだ名,Nickname" <-- only add if it あだ名 exists on the original file
		"MASTERNAME,How they adress their master," or "主人の呼び方,Name of Master," <-- only add if 主人の呼び方 is present on the original file
	2e) "CSTR,XX,Character string to be translated". Do note there's no "," on the end of the line of CSTR data. The XX number must be equal to the original number.
	2f) When the translation data for that character is complete, add a new line with "ENDCHR,XXX," with XXX as the number of the character
		You may also use "ENDCHR,," to the same effect, but the above is better for organization purposes

3) Add a new empty line and then go back to 2 for each new character translation added.
4) Save the file, check for errors, save again.

If correct, Emuera-Anchor should correctly translate the character data once a new game has been started.

==================================== 3 - TR-Related Commands ====================================

str TR_NAME (string code, int index)

	This command returns the translated (if it can't, returns the original) string based on code (ABL,TALENT,etc) and a index.
	The intended usage for this is when you need to manipulate translated strings, as Anchor will not translate things unless it's meant to be printed. An example would be working with CSTRs and the like, where they'll be saved into the save file and loaded again later, and Anchor won't be able to translate it.
	As some scripts may make use of CSTRs, you as the scripter should make sure to deal with the problems that could arise from using TR_NAME before publishing your changes.
	Also note that TRCHARA translated strings are always translated due to the nature of the beast, so there's no need for a character-equivalent command to exist.
	Some valid uses as follows:
		PRINTFORML %TR_NAME(ABL,"感性")%
		CSTR:99 = The string %TR_NAME("ABL",25)% is a translation of 感性

If you have any bug reports or comments or it just doesn't work @Bartoum or @JVN on the discord channel or post in the /hgg/ thread.