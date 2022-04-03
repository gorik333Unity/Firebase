using Firebase.Database;
using UnityEngine;
using Game.UI;
using System.Linq;
using System.Collections;

namespace Game.DB
{
    public class Database : MonoBehaviour
    {
        private const string USERS = "users";

        private DatabaseReference _databaseReference;

        private void Start()
        {
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void SaveData(string userName)
        {
            var user = new User(userName, Random.Range(20, 80), "On-line");
            string jsonUser = JsonUtility.ToJson(user);
            _databaseReference.Child(USERS).Child(userName).SetRawJsonValueAsync(jsonUser);
        }

        public void LoadData(string userName)
        {
            StartCoroutine(IELoadData(userName));
        }

        public void RemoveData(string userName)
        {
            var user = _databaseReference.Child(USERS).Child(userName).RemoveValueAsync();
        }

        public void ShowOrderedDataByChild(string value)
        {
            StartCoroutine(IEShowOrderedDataByChild(value));
        }

        private IEnumerator IEShowOrderedDataByChild(string value)
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

        private IEnumerator IELoadData(string userName)
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
