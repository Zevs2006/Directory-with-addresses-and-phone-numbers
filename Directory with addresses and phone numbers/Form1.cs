using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Directory_with_addresses_and_phone_numbers
{
    public partial class Form1 : Form
    {
        private List<Contact> contacts;
        private string filePath = "contacts.txt";
        private int selectedContactIndex = -1;

        public Form1()
        {
            InitializeComponent();
            contacts = new List<Contact>();
            LoadContacts();
        }

        // Метод для проверки правильности номера телефона
        private bool IsValidPhoneNumber(string phone)
        {
            // Пример проверки для российского номера: +7 (XXX) XXX-XX-XX
            string pattern = @"^\+7\s?\(?\d{3}\)?\s?\d{3}-\d{2}-\d{2}$";
            return Regex.IsMatch(phone, pattern);
        }

        // Добавить новый контакт
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string phone = txtPhone.Text;
            string address = txtAddress.Text;

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone))
            {
                // Проверяем правильность ввода номера телефона
                if (IsValidPhoneNumber(phone))
                {
                    Contact newContact = new Contact(name, phone, address);
                    contacts.Add(newContact);
                    UpdateContactList();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Неправильный формат номера телефона. Пример: +7 (123) 456-78-90");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните поля имени и телефона.");
            }
        }

        // Редактировать выбранный контакт
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedContactIndex != -1)
            {
                string name = txtName.Text;
                string phone = txtPhone.Text;
                string address = txtAddress.Text;

                // Проверяем правильность ввода номера телефона перед сохранением
                if (IsValidPhoneNumber(phone))
                {
                    contacts[selectedContactIndex].Name = name;
                    contacts[selectedContactIndex].Phone = phone;
                    contacts[selectedContactIndex].Address = address;

                    UpdateContactList();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Неправильный формат номера телефона. Пример: +7 (123) 456-78-90");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите контакт для редактирования.");
            }
        }

        // Остальные методы остаются без изменений

        // Удалить выбранный контакт
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedContactIndex != -1)
            {
                contacts.RemoveAt(selectedContactIndex);
                UpdateContactList();
                ClearFields();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите контакт для удаления.");
            }
        }

        // Сохранить контакты в файл
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveContacts();
            MessageBox.Show("Контакты сохранены.");
        }

        // Выбор контакта в списке
        private void lstContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstContacts.SelectedIndex != -1)
            {
                selectedContactIndex = lstContacts.SelectedIndex;
                Contact selectedContact = contacts[selectedContactIndex];
                txtName.Text = selectedContact.Name;
                txtPhone.Text = selectedContact.Phone;
                txtAddress.Text = selectedContact.Address;
            }
        }

        // Обновление списка контактов в ListBox
        private void UpdateContactList()
        {
            lstContacts.Items.Clear();
            foreach (Contact contact in contacts)
            {
                lstContacts.Items.Add($"{contact.Name} - {contact.Phone}");
            }
        }

        // Очистить текстовые поля
        private void ClearFields()
        {
            txtName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            selectedContactIndex = -1;
        }

        // Загрузка контактов из файла
        private void LoadContacts()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        Contact contact = new Contact(parts[0], parts[1], parts[2]);
                        contacts.Add(contact);
                    }
                }
                UpdateContactList();
            }
        }

        // Сохранение контактов в файл
        private void SaveContacts()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (Contact contact in contacts)
                {
                    writer.WriteLine($"{contact.Name},{contact.Phone},{contact.Address}");
                }
            }
        }
    }

    // Класс для хранения информации о контактах
    public class Contact
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public Contact(string name, string phone, string address)
        {
            Name = name;
            Phone = phone;
            Address = address;
        }
    }
}
