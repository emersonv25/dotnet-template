﻿using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Template.Infra.Data.Services
{
    public class FirebaseService
    {
        private readonly FirebaseApp _firebaseApp;
        private readonly string _firebaseCredentialPath;
        private readonly string _projectId;

        public FirebaseService(IConfiguration configuration)
        {
            _firebaseCredentialPath = configuration["AppSettings:FirebaseCredentialPath"]
                ?? throw new ArgumentNullException(nameof(_firebaseCredentialPath), "Firebase credential path cannot be null or empty.");

            var firebaseCredentialJson = File.ReadAllText(_firebaseCredentialPath);
            var firebaseCredentialJsonDoc = JsonDocument.Parse(firebaseCredentialJson);

            _projectId = firebaseCredentialJsonDoc.RootElement.GetProperty("project_id").GetString()
                ?? throw new ArgumentNullException(nameof(_projectId), "Project ID cannot be null or empty.");

            // Verifica se o FirebaseApp já foi inicializado
            if (FirebaseApp.DefaultInstance == null)
            {
                _firebaseApp = FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(_firebaseCredentialPath)
                });
            }
            else
            {
                _firebaseApp = FirebaseApp.DefaultInstance; // Usa a instância existente
            }
        }

        public string GetProjectId() => _projectId;

        public FirebaseApp GetFirebaseApp() => _firebaseApp;
    }
}
