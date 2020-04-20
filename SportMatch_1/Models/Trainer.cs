﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportMatch_1.Models
{
    public class Trainer
    {
        int trainerCode;
        string firstName;
        string lastName;
        string dateOfBirth;
        string email;
        string phone1; 
        string phone2;
        string gender;
        string password;
        string aboutMe;
        int pricePerHour;
        string image;


        public Trainer()
        {

        }

        public int TrainerCode { get => trainerCode; set => trainerCode = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public string Email { get => email; set => email = value; }
        public string Phone1 { get => phone1; set => phone1 = value; }
        public string Phone2 { get => phone2; set => phone2 = value; }
        public string Gender { get => gender; set => gender = value; }
        public string Password { get => password; set => password = value; }
        public string AboutMe { get => aboutMe; set => aboutMe = value; }
        public int PricePerHour { get => pricePerHour; set => pricePerHour = value; }
        public string Image { get => image; set => image = value; }

        public Trainer(string fn, string ln, string em, string ph1, string ph2, string gen, string pas, string abm, int pr, string dateofBirth, string img1)
        {
            FirstName = fn;
            LastName = ln;
            Email = em;
            Phone1 = ph1;
            Phone2 = ph2;
            Gender = gen;
            Password = pas;
            AboutMe = abm;
            PricePerHour = pr;
            DateOfBirth = dateOfBirth;
            Image = img1;
        }
        public Trainer insert()
        {
            DBservices dbs = new DBservices();
            Trainer numAffected = dbs.insertTrainer(this);
            return numAffected;
        }
        public List<Trainer> GetTrainerList()
        {
            DBservices dbs = new DBservices();
            List<Trainer> arrTrainers = dbs.GetTrainerList();
            return arrTrainers;
        }
    }
}