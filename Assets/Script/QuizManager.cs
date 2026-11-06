using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestionData {
        public string question;
        public string[] choices;
        public int correctAnswerIndex; // 0=A, 1=B, 2=C, 3=D
    }

    [Header("UI Elements")]
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public GameObject resultPanel;
    public TextMeshProUGUI scoreText;

    [Header("หน้าต่างแจ้งเตือน (กรณีเล่นด่านไม่ครบ)")]
    public GameObject notCompletedPanel; 

    [Header("ใส่คำถามทั้งหมดของทุกด่านเรียงกันที่นี่ได้เลย")]
    public List<QuestionData> allQuestions;

    private int currentQuestionIndex = 0;
    private int score = 0;

    void Start()
    {
        resultPanel.SetActive(false);
        if (notCompletedPanel != null) notCompletedPanel.SetActive(false);

        // 🌟 1. เช็คว่าผู้เล่นเคลียร์ด่านครบทั้ง 4 ด่านหรือยัง?
        if (CheckAllLevelsCompleted())
        {
            // ถ้าครบแล้ว เริ่มแสดงคำถามได้เลย
            ShowQuestion();
        }
        else
        {
            // ถ้ายังไม่ครบ ให้ซ่อนคำถาม แล้วโชว์หน้าแจ้งเตือนแทน
            questionText.gameObject.SetActive(false);
            foreach (Button btn in answerButtons) btn.gameObject.SetActive(false);
            if (notCompletedPanel != null) notCompletedPanel.SetActive(true);
        }
    }

    // ฟังก์ชันเช็คว่าผ่านครบทุกด่านไหม
    // ฟังก์ชันเช็คว่าเคยกดครบทุกด่านไหม (แก้ไขใหม่)
    bool CheckAllLevelsCompleted()
    {
        // 🌟 เปลี่ยนจากคำว่า Cleared เป็น Visited ให้ตรงกับที่เราจำไว้
        int eq = PlayerPrefs.GetInt("Earthquake_Visited", 0);
        int fl = PlayerPrefs.GetInt("Flood_Visited", 0);
        int st = PlayerPrefs.GetInt("Storm_Visited", 0);
        int fi = PlayerPrefs.GetInt("Fire_Visited", 0);

        return (eq == 1 && fl == 1 && st == 1 && fi == 1);
    }

    
    void ShowQuestion()
    {
        if (currentQuestionIndex < allQuestions.Count)
        {
            QuestionData q = allQuestions[currentQuestionIndex];
            questionText.text = q.question;

            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.choices[i];
                int index = i; 
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
            }
        }
        else
        {
            ShowResult();
        }
    }

    void CheckAnswer(int selectedIndex)
    {
        if (selectedIndex == allQuestions[currentQuestionIndex].correctAnswerIndex)
        {
            score++;
        }
        
        currentQuestionIndex++;
        ShowQuestion();
    }

    void ShowResult()
    {
        // ซ่อนคำถามและปุ่มตอบ
        questionText.gameObject.SetActive(false);
        foreach (Button btn in answerButtons) btn.gameObject.SetActive(false);
        
        // โชว์คะแนน
        resultPanel.SetActive(true);
        scoreText.text = $"คะแนน {score} / {allQuestions.Count}";
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Scene Selection");
    }
}