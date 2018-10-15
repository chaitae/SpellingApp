
using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UI; // Required when Using UI elements.



public class Email : MonoBehaviour {
    public InputField mainInputField;
    void Awake()
    {
        mainInputField = GetComponent<InputField>();
    }
    
    string WrapColorTag(string word, string color)
    {
        string temp = "";
        temp = "<font color =" + '"' + "color:"+color + '"' + ">" + word + "</font>";
        return temp;
    }

    string AddList()
    {
        string listBody = "";
        listBody = "<tr><th>Given</th><th>Entered</th></tr>";
        foreach(WordPair word in StringManager.Instance.answerPairs)
        {
            listBody += "<tr><td>" + word.correctSpelling + "</td>";
            listBody += "<td>" + WrapColorTag(word.enteredSpelling,"red") + "</tr></td>";

        }
        foreach (WordPair word in StringManager.Instance.answerPairsCorrect)
        {
            listBody += "<tr><td>" + word.correctSpelling + "</td>";
            listBody += "<td>" + WrapColorTag(word.correctSpelling, "blue") + "</tr></td>";
        }
        return listBody;
    }
    public void EmailPeople()
    {
        string headerString = WrapColorTag("List Tested: "+SpellingListManager.Instance.masterList[SpellingListManager.Instance.currIndex].name +" Date:"+ 
        DateTime.Now.Date, "black");
        string email;
        Debug.Log("inside the input" + mainInputField.text);
        email = mainInputField.text;
        Debug.Log(email);
        MailMessage mail = new MailMessage();
        mail.IsBodyHtml = true;
        mail.From = new MailAddress("WordPallearningtool@gmail.com");
        mail.To.Add(email);
        mail.Subject = "WordPal: TestScore "+SpellingListManager.Instance.masterList[SpellingListManager.Instance.currIndex];
        mail.Body =headerString+
        "<br>Here are the results from WordPal Learning App</br>" +
        "<br>" + "Got " + StringManager.Instance.correctAnswers + " words out of " + StringManager.Instance.tries +" correct.</br>" +AddList();

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("WordPallearningtool@gmail.com", "c0ugh8Drop!") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        Debug.Log("success");

    }
}
