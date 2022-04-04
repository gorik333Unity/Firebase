using Firebase.Database;
using Firebase.Auth;
using UnityEngine;
using Game.UI;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;

namespace Game.DB
{
    public class Database : MonoBehaviour
    {
        private const string USERS = "users";

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
            var user = new User(userName, Random.Range(20, 80), "On-line");
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
            // check if successed loging

            Debug.Log("Auth state changed");
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
                    Debug.Log(item.Child("Name").Value.ToString() + " " + item.Child("Age").Value.ToString());
            }
        }

        private IEnumerator TryLoadData(string userName)
        {
            var user = _databaseReference.Child(USERS).Child(userName).GetValueAsync();

            yield return new WaitUntil(() => user.IsCompleted);

            if (user.Result != null && user.Exception == null)
            {
                DataSnapshot snapshot = user.Result;

                Debug.Log(snapshot.Child("Age").Value.ToString() + " " + snapshot.Child("Name").Value.ToString());
            }
        }
    }

    public class User
    {
        public string Name;
        public int Age;
        public string Status;

        public User(string name, int age, string status)
        {
            Name = name;
            Age = age;
            Status = status;
        }
    }
}
