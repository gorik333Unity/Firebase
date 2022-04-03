using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.DB;

namespace Game.UI
{
    public class FirebaseTest : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _inputField;

        [SerializeField]
        private Database _database;

        /// <summary>
        /// Saves data from input field to database on click
        /// </summary>
        public void SaveDataOnClick()
        {
            _database.SaveData(GetAndResetInput(_inputField));
        }

        /// <summary>
        /// Loads data from input field from database on click
        /// </summary>
        public void LoadDataOnClick()
        {
            _database.LoadData(GetAndResetInput(_inputField));
        }

        /// <summary>
        /// Removes data from input field from database on click
        /// </summary>
        public void RemoveDataOnClick()
        {
            _database.RemoveData(GetAndResetInput(_inputField));
        }

        /// <summary>
        /// Gets and order data from input field from database on click
        /// </summary>
        public void ShowOrderdDataOnClick()
        {
            _database.ShowOrderedDataByChild(GetAndResetInput(_inputField));
        }

        private string GetAndResetInput(TMP_InputField inputField)
        {
            string input = inputField.text;
            inputField.text = string.Empty;

            return input;
        }
    }
}
