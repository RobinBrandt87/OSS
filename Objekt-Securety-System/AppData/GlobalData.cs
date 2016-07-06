using System;
using Windows.UI.Xaml;

namespace Objekt_Securety_System
{
    class GlobalData
    {
            private static bool myDebug = true;
            public static bool MyDebug
            {
                get { return myDebug; }
                set { myDebug = value; }
            }
        private static string uri2 = "";
        public static string Uri2
        {
            get { return uri2; }
            set { uri2 = value; }   
        }
        private static int i = 0;
        public static int I
        {
            get { return i; }
            set { i = value; }
        }
        private static string login = "";
        public static string Login
        {
            get { return login; }
            set { login = value; }
        }
        private static string rute = "";
        public static string Rute
        {
            get { return rute; }
            set { rute = value; }
        }
        private static string passwd;
        public static string Passwd
        {
            get { return passwd; }
            set { passwd = value; }
        }
        private static string passwdsha1;
        public static string Passwdsha1
        {
            get { return passwdsha1; }
            set { passwdsha1 = value; }
        }
        private static string sessionid;
        public static string SessionID
        {
            get { return sessionid; }
            set { sessionid = value; }
        }
        private static string stringpasswd;
        public static string StringPasswd
        {
            get { return stringpasswd; }
            set { stringpasswd = value; }
        }
        private static string httpresponse;
        public static string HttpResponse
        {
            get { return httpresponse; }
            set { httpresponse = value; }
        }
        private static string pers_id;
        public static string Pers_ID
        {
            get { return pers_id; }
            set { pers_id = value; }
        }
        private static string antwort ="";
        public static string Antwort
        {
            get { return antwort; }
            set { antwort = value; }
        }
        private static string tag_id = "";
        public static string Tag_ID
        {
            get { return tag_id; }
            set { tag_id = value; }
        }
        private static string tag_id_alt = "";
        public static string Tag_ID_Alt
        {
            get { return tag_id_alt; }
            set { tag_id_alt = value; }
        }
        private static bool check;
        public static bool Check
        {
            get { return check; }
            set { check = value; }
        }
        private static string plan_id;
        public static string Plan_ID
        {
            get { return plan_id; }
            set { plan_id = value; }
        }
        private static string routen_id;
        public static string Routen_ID
        {
            get { return routen_id; }
            set { routen_id = value; }
        }
        private static string startzeit;
        public static string Startzeit
        {
            get { return startzeit; }
            set { startzeit = value; }
        }
        private static DateTime routinenzeit;
        public static DateTime Routinenzeit
        {
            get { return routinenzeit; }
            set { routinenzeit = value; }
        }
        private static string tag_id2;
        public static string Tag_ID2
        {
            get { return tag_id2; }
            set { tag_id2 = value; }
        }
        private static bool timer_enable;
        public static bool Timer_Enable
        {
            get { return timer_enable; }
            set { timer_enable = value; }
        }
        private static bool login_enable;
        public static bool Login_Enable
        {
            get { return login_enable; }
            set { login_enable = value; }
        }
        private string[,] Routen = new string[64, 4];

    }
}
