
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;//This allows you read in and write to files
using System.Threading;//This allows you to use Thread.Sleep(2000) method which pauses the processing for n seconds 
using System.Diagnostics;

namespace GeographyFT
{
    class Program
    {
        //These are global variables and if they are declared outside the main method and inside the class
        //it means they can be accessed/referenced from anywhere in the program
        static string optionEntered;//This will store the menu option chosen by the user in each menu
        static int validNumber = 0;//This will store the number entered once it has been properly validated
        static int maxMenu = 0;//This is the max number of menu options within the particular menu
        static string path = "students.csv";//This file is stored in your bin>debug folder within your solution(in solution explorer click view all files
        static string[] users = new string[File.ReadAllLines(path).Length];//This create a 1D array capable of storing all user records from the student.csv file
        static int noStudents = users.Length;//This checks the number of students records in the users array
        static int numberOfBitsOfInfo = 11;//The number of columns (fields) in the file.
        //This 2D array will store the student details in working memory (RAM) while the program is running.
        static string[,] students = new string[noStudents, numberOfBitsOfInfo];

        //This is your main method.
        //This is the executable part of your program. Your applications begins from here.
        //The main method controls the program flow of control.
        static void Main(string[] args)
        {
            readInUsers();//This populates the student list with the data from the students.csv file
            login();//This launches the main menu from where the user can choose menu options       
            Process aProcess = new Process();
            aProcess.Start();
        }

        //This method will read in users from the CSV file to the 1D array users created as a static variable
        static void readInUsers()
        {
            //A try...catch will ensure any exceptions are 'caught' and handled appropriately.
            try
            {
                //This reads in the users into an array from file path location specified
                users = File.ReadAllLines(path);
            }

            //FileNotFoundException is derived from the Exception class
            catch (FileNotFoundException fnfex)
            {
              //In a form-based applicatioon, this could be replaced with MessageBox.Show(....)
                Console.WriteLine("File not found. Type any ket to continue" + fnfex.ToString());
            }
               

            //This array will store each students details split during the outer for loop
            string[] tempStudent = new string[numberOfBitsOfInfo];

            //This nested for loop will iterate through the 1D array and populate the 2D array 
          
            //The outer for controls the iteration through the rows
            for (int i = 0; i < noStudents; i++)
            {
                tempStudent = users[i].Split(',');
                for (int j = 0; j < numberOfBitsOfInfo; j++)
                {
                    students[i, j] = tempStudent[j];
                }
            }
        }


        //This is a complex login function
        static void login()
        {
          bool notLoggedIn= true;
          string username, password;
          int loginAttempts=0;

          do
          {
            appTitle("Login menu");
            Console.WriteLine("Please enter your username");
            loginAttempts++;
            
              do
              {
                username = Console.ReadLine();
              }
              while(string.IsNullOrEmpty(username));

              Console.WriteLine("Please enter your password");
              password = Console.ReadLine();


              for(int i=0;i<students.GetLength(0);i++)
                {
                  if(students[i,0]==username)
                    if(students[i,1]==password)
                      {
                        Console.WriteLine("You have successfully logged in");
                        Thread.Sleep(2000);
                        mainMenu();
                      }
                    else
                      {
                        Console.WriteLine("Your username or password is incorrect. Please try again");
                        Thread.Sleep(2000);
                        login();
                      }
                  }
              }
         while(notLoggedIn); 
         
        }
        //This method will check to make sure the user has entered a valid menu option
        static bool validOption(string optionEntered, int maxValue)
        {
            bool isValid = false;
            //This converts a string to an int so long as that's possible e.g. does not contain letters
            if ((int.TryParse(optionEntered, out validNumber)) && (validNumber <= maxValue))
                isValid = true;
            else
                isValid = false;

            return isValid;
        }

        
        static void writeToFile(string[,] newStudents)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    for (int i = 0; i < newStudents.GetLength(0); i++)
                    {
                        for (int j = 0; j < newStudents.GetLength(1); j++)
                        {
                            sw.Write(newStudents[i, j] + ",");
                        }
                        sw.WriteLine();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("The file did not write correctly" + ex.ToString());
                mainMenu();
            }
            Console.WriteLine("Students details were succesfully updated. Press any key to continue");
            Console.ReadKey();
        }

        //This method gives each screen a consistent look and feel. It is called during each new screen and displays the screen name sent to it by the calling method
        static void appTitle(string menuName)
        {
            Console.Clear();
            Console.WriteLine("*****FRIENDS' SCHOOL LISBURN*****");
            Console.WriteLine("*****GEOGRAPHY FIELD TRIP********");
            Console.WriteLine("*********************************");
            Console.WriteLine("You are here: " + menuName);
        }

        static void mainMenu()
        {
            appTitle("Main Menu");
            readInUsers();
            Console.WriteLine("1. Add Student Details");
            Console.WriteLine("2. Delete a student");
            Console.WriteLine("3. Geography Quiz");
            Console.WriteLine("4. Finance");
            Console.WriteLine("5. Reporting");
            maxMenu = 5;
            try
            {
                do
                {
                    Console.WriteLine("Enter menu option");
                    optionEntered = Console.ReadLine();
                }
                while (!validOption(optionEntered, maxMenu));
            }
            catch (Exception ex)
            {
                Console.WriteLine("The following issue ocurred " + ex.ToString());
                mainMenu();
            }

            switch (validNumber)
            {
                case 1:
                    addStud();
                    break;
                case 2:
                    deleteStudent();
                    break;
                case 3:
                    quizMenu();
                    break;
                case 4:
                    financeMenu();
                    break;
                case 5:
                    reports();
                    break;
            }
        }

        //This method will enable the user to add a new student to the 2D student list.
        //It will not add either deposits or quiz scores, which will be initialised to 0
        static void addStud()
        {
            string firstName = "";
            string surname = "";
            string gender = "";
            string house = "";
            string username = "";
            int age = 0;
            int deposit1 = 0;
            int deposit2 = 0;
            int deposit3 = 0;
            int quizScore = 0;

            appTitle("ADD STUDENT DETAILS");

            try
            {
                Console.WriteLine("Please enter student first name");
                firstName = Console.ReadLine();

                Console.WriteLine("Please enter student surname");
                surname = Console.ReadLine();
                do
                {
                    Console.WriteLine("Please enter gender (\"m\" or \"f\"");
                    gender = Console.ReadLine().ToLower();
                }
                while (((gender != "m") && (gender != "f")));

                do
                {
                    Console.WriteLine("Please enter age");
                }
                while (!((int.TryParse(Console.ReadLine(), out age) && (age >= 11 && age <= 18))));

                do
                {
                    Console.WriteLine("Which HOUSE is the student a member of: \n\"Collin\" \n\"Croob\"\n\"Divis\" \n\"Aughrim\"");
                    house = Console.ReadLine().ToLower();
                }
                while (((house != "collin") && (house != "aughrim") && (house != "divis") && (house != "croob")));
            }
            catch (Exception ex)
            {
                Console.WriteLine("The following error occurred " + ex.ToString());
                addStud();
            }

            string[] tempStudent = new string[numberOfBitsOfInfo];

            username = generateUsername(firstName, surname);
            tempStudent[0] = username;
            tempStudent[1] = "password123";
            tempStudent[2] = firstName;
            tempStudent[3] = surname;
            tempStudent[4] = gender;
            tempStudent[5] = age.ToString();
            tempStudent[6] = house;
            tempStudent[7] = deposit1.ToString();
            tempStudent[8] = deposit2.ToString();
            tempStudent[9] = deposit3.ToString();
            tempStudent[10] = quizScore.ToString();


            //Increase student array by one to make room for additional student
            string[,] newStudents = new string[students.GetLength(0) + 1, numberOfBitsOfInfo];

            for (int i = 0; i < students.GetLength(0); i++)
                for (int j = 0; j < students.GetLength(1); j++)
                {
                    newStudents[i, j] = students[i, j];
                }
            
            for (int k = 0; k < numberOfBitsOfInfo; k++)
            {
                newStudents[newStudents.GetLength(0) - 1, k] = tempStudent[k];
            }
            writeToFile(newStudents);
            Console.WriteLine("New student details have been written to file. Press any key to return to the main menu");
            Console.ReadKey();
            mainMenu();

        }
        //Delete a student
        static void deleteStudent()
        {
          appTitle("DELETE A STUDENT");
          string username;
          int failCounter=0;
          bool deleteAnother=true;
          do
          {
            do
            {
              if(failCounter==0)
              {
                Console.WriteLine("Please enter username of the student you wish to delete");
                username= Console.ReadLine();
                failCounter++;
              }

              else
              {
                Console.WriteLine("Username not found please reenter username you wish to delete .");
                username= Console.ReadLine();
              }
            }
            while(notFound(username));
            
            //This creates a new array to store one less student
            string[,] removeStudent = new string[students.GetLength(0)-1,numberOfBitsOfInfo];
            
            //This iterates through the 2D array until it find the user match
            for(int i=0;i<students.GetLength(0)-1;i++)
              {
                if(students[i,0]==username)
                {   
                  for (int j =i;j<students.GetLength(0)-1;j++)
                    for(int k=0;k<students.GetLength(1);k++)
                      {
                        students[j,k]=students[j+1,k];
                      }
                }

              }
      
              for(int a =0;a<removeStudent.GetLength(0);a++)
                for(int b=0;b<removeStudent.GetLength(1);b++)
                {
                        removeStudent[a,b]=students[a,b];
                }
            writeToFile(removeStudent);
            Console.WriteLine("Would you like to delete another student");
            char response = Convert.ToChar(Console.ReadKey());
            if(response=='y')
              deleteAnother=false;
            else
              deleteAnother=true;
              }
        while(deleteAnother);
        mainMenu();
        }

        static bool notFound(string username )
        {
          for(int i=0;i<students.GetLength(0);i++)
          {
            if(students[i,0]==username)
               { 
                  return false;
                  break;
               }
          }

          return true;
        }
        //This short method will generate a username name by taking the first lettter of the first name and surname and a 3-digit random number
        static string generateUsername(string firstName, string surname)
        {
            string username;
            Random rand = new Random();
            int randomNumber = rand.Next(100, 999);
            username = firstName.Substring(0, 1) + surname + randomNumber.ToString();
            return username;
        }

        static void quizMenu()
        {
            appTitle("GEOGRAPHY QUIZ MENU");
            Console.WriteLine("1. Start Quiz");
            Console.WriteLine("2. Leaderboard");
            Console.WriteLine("3. Back to Main Menu");

            try
            {
                do
                {
                    Console.WriteLine("Enter menu option");
                    optionEntered = Console.ReadLine();
                }
                while ((!validOption(optionEntered, maxMenu)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("The following issue ocurred " + ex.ToString());
                mainMenu();
            }

            switch (validNumber)
            {
                case 1:
                    geogQuiz();
                    break;
                case 2:
                    leaderboard();
                    break;
                case 3:
                    mainMenu();
                    break;
                default:
                    Console.WriteLine("You chose an invalid option. Please try again");
                    Thread.Sleep(3000);
                    quizMenu();
                    break;
            }
        }

        //This method will first of all determine which user is logged in to do the quiz, pulling their previous best score from the 2D array
        //This will then ask the user 5 questions checking if if each is correct and totalling their score.
        //If their score if better than their previous best, it will overwrite this score and tell the user.
        static void geogQuiz()
        {
            char correctAnswer, answerChosen;
            int quizScore = 0;
            appTitle("GEOGRAPHY QUIZ");
            string user = "";
            bool invalid = true;
            int previousBestScore = 0;
            int failCounter = 0;
            do
            {
                if (failCounter > 0)
                {
                    Console.WriteLine("Username does not exist. Please reenter username");
                }
                else
                {
                    Console.WriteLine("Please enter username of person taking the quiz");
                }
                user = Console.ReadLine().ToLower();
                for (int i = 0; i < students.GetLength(0); i++)
                {
                    if (user == students[i, 0])
                    {
                        invalid = false;
                        previousBestScore = Convert.ToInt16(students[i, 9]);
                    }
                }
                failCounter++;

            }
            while (invalid);

            Console.WriteLine("Welcome " + user + ", press any key to start quiz. Your previous best score was  " + previousBestScore + ".");
            Console.ReadKey();
            Thread.Sleep(2000);
            appTitle("QUIZ: QUESTION 1");
            Console.WriteLine("1. What is the capital of France");
            Console.WriteLine("a.Paris\nb.Marseille\nc.Nantes\nd.Dieppe");

            try
            {
                answerChosen = Convert.ToChar(Console.ReadLine().ToLower());
                correctAnswer = 'a';
                quizScore += checkQuestion(answerChosen, correctAnswer);

                appTitle("QUIZ: QUESTION 2");
                Console.WriteLine("2. In which US state can the city of Miami be found?");
                Console.WriteLine("a.New York\nb.California\nc.Colorado\nd.Florida");
                answerChosen = Convert.ToChar(Console.ReadLine().ToLower());
                correctAnswer = 'd';
                quizScore += checkQuestion(answerChosen, correctAnswer);

                appTitle("QUIZ: QUESTION 3");
                Console.WriteLine("3. On which continent can the Andes be found on");
                Console.WriteLine("a.North America\nb.South America\nc.Europe\nd.Africa");
                answerChosen = Convert.ToChar(Console.ReadLine().ToLower());
                correctAnswer = 'b';
                quizScore += checkQuestion(answerChosen, correctAnswer);

                appTitle("QUIZ: QUESTION 4");
                Console.WriteLine("4. On which continent can Mt Kilmanjiro be found");
                Console.WriteLine("a.North America\nb.South America\nc.Europe\nd.Africa");
                answerChosen = Convert.ToChar(Console.ReadLine().ToLower());
                correctAnswer = 'd';
                quizScore += checkQuestion(answerChosen, correctAnswer);

                appTitle("QUIZ: QUESTION 5");
                Console.WriteLine("5. Which is the largest mountain in the world");
                Console.WriteLine("a.Everest\nb.K2\nc.Mount St Helens\nd.black");
                answerChosen = Convert.ToChar(Console.ReadLine().ToLower());
                correctAnswer = 'a';
                quizScore += checkQuestion(answerChosen, correctAnswer);
            }
            catch(Exception ex)
            {
                Console.WriteLine("The following error occurred " + ex.ToString() + ". Press any key to continue");
                Console.ReadKey();
                geogQuiz();
            }

            appTitle("END OF QUIZ");

            for (int i = 0; i < students.GetLength(0); i++)
                if (user == students[i, 0])
                {
                    if (quizScore > (Convert.ToInt16(students[i, 9])))
                    {
                        students[i, 9] = quizScore.ToString();
                        writeToFile(students);
                        Console.WriteLine("Congratulations, you scored " + quizScore + ". This is your best ever score. Press any key to return to Main Menu");
                        Console.ReadKey();
                        mainMenu();
                    }
               
                    else
                    {
                      Console.WriteLine("You scored " + quizScore + " but failed to beat your previous best. Press any key to return to Main Menu");
                      Console.ReadKey();
                      mainMenu();
                    }
                }
        }

        //This method will check if the question is correct or not and return a score for correctly answering the question.
        static int checkQuestion(char answerChosen, char correctAnswer)
        {
            int userScore = 0;

            if (correctAnswer == answerChosen)
            {
                userScore = userScore + 1;
                Console.WriteLine("Well done. Correct answer");
            }
            else
                Console.WriteLine("Hard luck. Wrong answer. The correct answer was " + correctAnswer);
            return userScore;

        }

        //This method will create a smaller 2D array (quizScore) for use in the leaderboard
        //Using a BUBBLE SORT algorithm, this method will subsequently sort the quiz score by the quiz olumn from highest to lowest
        //For more information on a Bubble Sort, i'll post up some information soon.
        static void leaderboard()
        {
            appTitle("GEOGRAPHY QUIZ LEADERBOARD");
            string[,] quizScore = new string[noStudents, 3];

            for (int i = 0; i < students.GetLength(0); i++)
            {
                quizScore[i, 0] = students[i, 2];
                quizScore[i, 1] = students[i, 3];
                quizScore[i, 2] = students[i, 10];
            }
            string temp, temp1, temp2;
            for (int j = 0; j <= quizScore.GetLength(0) - 1; j++)
            {
                for (int i = 0; i < quizScore.GetLength(0) - 1; i++)
                {

                    if (Convert.ToInt16((quizScore[i, 2])) < Convert.ToInt16((quizScore[i + 1, 2])))
                    {
                        temp = quizScore[i + 1, 0];
                        temp1 = quizScore[i + 1, 1];
                        temp2 = quizScore[i + 1, 2];

                        quizScore[i + 1, 0] = quizScore[i, 0];
                        quizScore[i + 1, 1] = quizScore[i, 1];
                        quizScore[i + 1, 2] = quizScore[i, 2];

                        quizScore[i, 0] = temp;
                        quizScore[i, 1] = temp1;
                        quizScore[i, 2] = temp2;
                    }
                }
            }
            int rank = 1;
            Console.WriteLine("Rank\tName\t\tScore");
            for (int a = 0; a < quizScore.GetLength(0); a++)
            { 
                Console.Write(rank + "\t\t"+ quizScore[a, 0].ToString() + "\t\t" + quizScore[a, 1].ToString() + "\t\t");
                Console.Write((quizScore[a, 2].ToString()) + "\t\t");
                Console.WriteLine();
                rank++;
            }
            Console.WriteLine("Press any key to return to the Quiz menu");
            Console.ReadKey();
            quizMenu();
        }

        static void financeMenu()
        {
            appTitle("FINANCE MENU");
            Console.WriteLine("1. Add Deposit");
            Console.WriteLine("2. Total Deposit Paid");
            Console.WriteLine("3. Return to Main Menu");
            maxMenu = 4;
            do
            {
                Console.WriteLine("Enter menu option");
                optionEntered = Console.ReadLine();
            }
            while (!validOption(optionEntered, maxMenu));

            switch (validNumber)
            {
                case 1:
                    addDeposit();
                    break;
                case 2:
                    totalDeposit();
                    break;
                case 3:
                    mainMenu();
                    break;
            }
        }

        //This function will enable the program to add the three deposits paid by student username
        //This method will first determine if the username is a valid user i.e. exists in column 0 in teh student list 2D array
        static void addDeposit()
        {
            appTitle("ADD DEPOSIT");
            bool invalidUser = true;
            int usernameRowID = 0;
            int deposit;
            string currentValidUser = "";
            
              do
              {

                try
                {
                  Console.WriteLine("Please enter your username");
                  string user = Console.ReadLine();
                  readInUsers();

                  //Check if user exists in student users file
                  for (int i = 0; i < students.GetLength(0); i++)
                      if (students[i, 0] == user)
                      {
                          invalidUser = false;
                          currentValidUser = students[i, 0];
                          usernameRowID = i;
                      }
                }
            
              catch(Exception ex)
              {
                Console.WriteLine("The following issue occurred " + ex.ToString() + ". \nPress any key to reload the add deposit menu");
                Console.ReadKey();
                addDeposit();
              }
            }
            while (invalidUser);
            
            Console.WriteLine("You are about to add deposits for " + currentValidUser + ". Please enter deposit amount");
            for (int j = 1; j <= 3; j++)
            {
                do
                {

                    Console.WriteLine("Enter deposit amount " + j + ":");
                }
                while (!int.TryParse(Console.ReadLine(), out deposit));
                students[usernameRowID, j + 5] = deposit.ToString();
            }

            writeToFile(students);
            Console.WriteLine("Ski times successfully updated. Press any key to return to ski menu");
            Console.ReadKey();
            financeMenu();
        }

        //This method will calculate the total amount of the deposit paid for each student and the amount outstanding and print these calculations out
        static void totalDeposit()
        {
            appTitle("TOTAL DEPOSIT PAID");
            double totalDepositPaid;
            string[,] studentDeposits = new string[students.GetLength(0), 3];
            for (int i = 0; i < students.GetLength(0); i++)
            {
                totalDepositPaid = 0;
                for (int j = 6; j <=8; j++)
                {
                    totalDepositPaid += Convert.ToDouble(students[i, j]);
                }
                studentDeposits[i, 0] = students[i, 1];
                studentDeposits[i, 1] = students[i, 2];
                studentDeposits[i, 2] = totalDepositPaid.ToString();
            }

            string temp, temp1, temp2;
            for (int j = 0; j <= studentDeposits.GetLength(0) - 1; j++)
            {
                for (int i = 0; i < studentDeposits.GetLength(0) - 1; i++)
                {

                    if (double.Parse(studentDeposits[i, 2]) > double.Parse(studentDeposits[i + 1, 2]))
                    {
                        temp = studentDeposits[i + 1, 0];
                        temp1 = studentDeposits[i + 1, 1];
                        temp2 = studentDeposits[i + 1, 2];

                        studentDeposits[i + 1, 0] = studentDeposits[i, 0];
                        studentDeposits[i + 1, 1] = studentDeposits[i, 1];
                        studentDeposits[i + 1, 2] = studentDeposits[i, 2];

                        studentDeposits[i, 0] = temp;
                        studentDeposits[i, 1] = temp1;
                        studentDeposits[i, 2] = temp2;
                    }
                }
            }
            Console.WriteLine("Name\t\tAmount Paid\tAmount Outstanding");
            for (int a = 0; a < studentDeposits.GetLength(0); a++)
                {
                    Console.WriteLine(studentDeposits[a, 0].ToString() +"\t"+ studentDeposits[a, 1].ToString() +"\t\t£"+ studentDeposits[a, 2].ToString() + "\t£" + (100-(Convert.ToDouble(studentDeposits[a,2]))));
                }
                Console.WriteLine();
            
            Console.WriteLine("Press any key to return to the Finance menu");
            Console.ReadKey();
            financeMenu();
        }

        //This method will iterate through the 2D student list and print the first name (column 1) and surname (column 2) of students grouping them by house
        static void studentHouses()
        {
            appTitle("VIEW STUDENTS BY HOUSE");
            Console.WriteLine("\t\tCOLLIN");
            for (int i = 0; i < students.GetLength(0); i++)
            {
                if (students[i, 6] == "Collin")
                {
                    Console.WriteLine(students[i, 1] + "\t" + students[i, 2]);
                }
            }
            Console.WriteLine("\n\t\tAUGRHIM");
            for (int i = 0; i < students.GetLength(0); i++)
            {
                if (students[i, 6] == "aughrim")
                {
                    Console.WriteLine(students[i, 1] + "\t" + students[i, 2]);
                }
            }
            Console.WriteLine("\n\t\tCROOB");
            for (int i = 0; i < students.GetLength(0); i++)
            {
                if (students[i, 5] == "croob")
                {
                    Console.WriteLine(students[i, 1] + "\t" + students[i, 2]);
                }
            }
            Console.WriteLine("\n\t\tDIVIS");
            for (int i = 0; i < students.GetLength(0); i++)
            {
                if (students[i, 5] == "divis")
                {
                    Console.WriteLine(students[i, 1] + "\t" + students[i, 2]);
                }
            }
            Console.WriteLine("Press any key to return to the Finance menu");
            Console.ReadKey();
            financeMenu();

        }

        static void reports()
        {
            appTitle("REPORTS");
            Console.WriteLine("1. Students by age group ");
            Console.WriteLine("2. Students by gender");
            Console.WriteLine("3. List of students");
            Console.WriteLine("4. Return to main menu ");
            maxMenu = 4;
            do
            {
                Console.WriteLine("Enter menu option");
                optionEntered = Console.ReadLine();
            }
            while (!validOption(optionEntered, maxMenu));

            switch (validNumber)
            {
                case 1:
                    ageGroup();
                    break;
                case 2:
                    gender();
                    break;
                case 3:
                    studentList();
                    break;
                case 4:
                    mainMenu();
                    break;
            }
        }

        static void ageGroup()
        {
            appTitle("STUDENTS BY AGE GROUP\n");
            Console.WriteLine("\t\tAGE 11");
            for (int i = 0; i < students.GetLength(0); i++)
            {
                if ((Int16.Parse(students[i, 5]) >= 11) && (Int16.Parse(students[i, 5]) <= 11))
                {
                    Console.WriteLine(students[i, 2] + "\t" + students[i, 3]);
                }
            }
            Console.WriteLine("\n\t\tAGE 12");
            for (int i = 0; i < students.GetLength(0); i++)
            {
                if ((Int16.Parse(students[i, 5]) >= 12) && (Int16.Parse(students[i, 5]) <= 12))
                {
                    Console.WriteLine(students[i, 2] + "\t" + students[i, 3]);
                }
            }
            Console.WriteLine("\nPress any key to return to the reports menu");
            Console.ReadKey();
            reports();
        }

        //This method will iterate through the 2D student list and print the names of students who are male and female in to distinct lists
        static void gender()
        {
            appTitle("STUDENTS BY GENDER\n");
            Console.WriteLine("\t\tMALE");
            for (int i = 0; i < students.GetLength(0); i++)
            {
                if (students[i, 4] == "m")
                {
                    Console.WriteLine(students[i, 2] + "\t" + students[i, 3]);
                }
            }
            Console.WriteLine("\n\t\tFEMALE");
            for (int i = 0; i < students.GetLength(0); i++)
            {
                if (students[i, 4] == "f")
                {
                    Console.WriteLine(students[i, 2] + "\t" + students[i, 3]);
                }
            }
            Console.WriteLine("\nPress any key to return to the reports menu");
            Console.ReadKey();
            reports();
        }

        //This method will use a nested for loop to iterate through each student record and printing the first 5 columns (excluding username)
        static void studentList()
        {
            appTitle("STUDENTS LIST\n");
            Console.WriteLine("Name\t\tGender\tAge");
            for (int i = 0; i < students.GetLength(0); i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    Console.Write(students[i, j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\nPress any key to return to the reports menu");
            Console.ReadKey();
            reports();
        }
    }
}
