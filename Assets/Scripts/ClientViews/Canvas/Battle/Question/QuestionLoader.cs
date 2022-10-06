using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestionForm{
    public int id;
    public string question;
    public List<string> answers;
    public string correctAnswer;

    public QuestionForm(int id, string question, List<string> ans, string correctAnswer){
        this.id = id;
        this.question = question;
        this.answers = ans;
        this.correctAnswer = correctAnswer;
    }
}

public class QuestionLoader
{
    public static string questionPath = "Assets/StreamingAssets/demo_question.csv";
    public static List<QuestionForm> LoadQuestion(){
        List<QuestionForm> questions = new List<QuestionForm>();
        string [] lines = System.IO.File.ReadAllLines(questionPath);
        foreach(string line in lines){
            string[] col = line.Split(',');
            QuestionForm que = new QuestionForm(System.Int32.Parse(col[0]), col[1], new List<string>{col[2], col[3], col[4], col[5]}, col[6]);
            questions.Add(que);
        }
        return questions;
    }

}
