using Firebase.Database;
using Firebase.Auth;
using UnityEngine;
using System.Threading.Tasks;
using System;

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
            var task = TrySignInAsync(email, password);
        }

        public void SignUp(string email, string password)
        {
            var task = TrySignUpAsync(email, password);
        }

        public void SaveData(string userName)
        {
            var task = TrySaveData(userName);
        }

        public void LoadData(string userName)
        {
            var task = TryLoadDataAsync(userName);
        }

        public void RemoveData(string userName)
        {
            var task = TryRemoveData(userName);
        }

        public void ShowOrderedDataByChild(string value)
        {
            var task = TryShowOrderedDataByChildAsync(value);
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
            Debug.Log("Auth state changed");
        }

        private async Task TrySaveData(string userName)
        {
            try
            {
                var user = new User(userName, UnityEngine.Random.Range(MIN_YEARS, MAX_YEARS), STATE);
                string jsonUser = JsonUtility.ToJson(user);

                await _databaseReference.Child(USERS).Child(userName).SetRawJsonValueAsync(jsonUser);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task TryRemoveData(string userName)
        {
            try
            {
                await _databaseReference.Child(USERS).Child(userName).RemoveValueAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task TrySignInAsync(string email, string password)
        {
            try
            {
                var user = _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
                await user;

                if (user.Result != null)
                {
                    Debug.Log("Success sign in");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task TrySignUpAsync(string email, string password)
        {
            try
            {
                var user = _firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);
                await user;

                if (user.Result != null)
                {
                    Debug.Log("Success sign up");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task TryShowOrderedDataByChildAsync(string value)
        {
            try
            {
                var users = _databaseReference.Child(USERS).OrderByChild(value).GetValueAsync();
                await users;

                if (users.Result != null)
                {
                    DataSnapshot snapshot = users.Result;

                    foreach (var item in snapshot.Children)
                    {
                        Debug.Log(item.Child(NAME).Value.ToString() + " " + item.Child(AGE).Value.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task TryLoadDataAsync(string userName)
        {
            try
            {
                var user = _databaseReference.Child(USERS).Child(userName).GetValueAsync();
                await user;

                if (user.Result != null)
                {
                    DataSnapshot snapshot = user.Result;

                    Debug.Log(snapshot.Child(NAME).Value.ToString() + " " + snapshot.Child(AGE).Value.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
