using Firebase.Database;
using Firebase.Auth;
using UnityEngine;
using System.Collections;

namespace Game.DB
{
    public class Database : MonoBehaviour
    {
        private const string USERS = "users";
        private const string NAME = "Name";
        private const string AGE = "Age";
        private const string STATE = "On-line";

        private const int MIN_YEARS = 20;
        private const int MAX_YEARS = 80;

        private DatabaseReference _databaseReference;
        private FirebaseAuth _firebaseAuth;

        public void SignIn(string email, string password)
        {
            StartCoroutine(TrySignIn(email, password));
        }

        public void SignUp(string email, string password)
        {
            StartCoroutine(TrySignUp(email, password));
        }

        public void SaveData(string userName)
        {
            var user = new User(userName, Random.Range(MIN_YEARS, MAX_YEARS), STATE);
            string jsonUser = JsonUtility.ToJson(user);
            _databaseReference.Child(USERS).Child(userName).SetRawJsonValueAsync(jsonUser);
        }

        public void LoadData(string userName)
        {
            StartCoroutine(TryLoadData(userName));
        }

        public void RemoveData(string userName)
        {
            _databaseReference.Child(USERS).Child(userName).RemoveValueAsync();
        }

        public void ShowOrderedDataByChild(string value)
        {
            StartCoroutine(TryShowOrderedDataByChild(value));
        }

        private void Awake()
        {
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            _firebaseAuth = FirebaseAuth.DefaultInstance;
        }

        private void Start()
        {
            _firebaseAuth.StateChanged += FirebaseAuth_StateChanged;

            _firebaseAuth.SignOut(); // if don't want auto sing in on application start
        }

        private void FirebaseAuth_StateChanged(object sender, System.EventArgs e)
        {
            Debug.Log("check if successed loging");
        }

        private IEnumerator TrySignIn(string email, string password)
        {
            var user = _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => user.IsCompleted);

            if (user.Result != null && user.Exception == null)
            {
                Debug.Log("Success sign in");
            }
        }

        private IEnumerator TrySignUp(string email, string password)
        {
            var user = _firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => user.IsCompleted);

            if (user.Result != null && user.Exception == null)
            {
                Debug.Log("Success sign up");
            }
        }

        private IEnumerator TryShowOrderedDataByChild(string value)
        {
            var users = _databaseReference.Child(USERS).OrderByChild(value).GetValueAsync();

            yield return new WaitUntil(() => users.IsCompleted);

            if (users.Result != null && users.Exception == null)
            {
                DataSnapshot snapshot = users.Result;

                foreach (var item in snapshot.Children)
                {
                    Debug.Log(item.Child(NAME).Value.ToString() + " " + item.Child(AGE).Value.ToString());
                }
            }
        }

        private IEnumerator TryLoadData(string userName)
        {
            var user = _databaseReference.Child(USERS).Child(userName).GetValueAsync();

            yield return new WaitUntil(() => user.IsCompleted);

            if (user.Result != null && user.Exception == null)
            {
                DataSnapshot snapshot = user.Result;

                Debug.Log(snapshot.Child(NAME).Value.ToString() + " " + snapshot.Child(AGE).Value.ToString());
            }
        }
    }
}
