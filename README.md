# Muether-Meyer-Nachhilfe

Dieses Projekt ist eine WPF-Anwendung zur Verwaltung von Nachhilfegesuchen und Benutzern (Schüler, Tutor und Administrator). Die Anwendung nutzt MySQL für die Datenspeicherung. Weitere Informationen, wie z.B. Login, Registrierung und Fachverwaltung, werden über die Klassen in `Muether-Meyer-Nachhilfe/common` bereitgestellt.

## Inhaltsverzeichnis
- [Überblick](#überblick)
- [Features](#features)
- [Storyboard](#storyboard)
- [Use Case](#usecase)

## Überblick

Das Projekt dient als Verwaltungssystem für Nachhilfesuchende. Es ermöglicht:
- Die Registrierung und den Login von Benutzern.
- Die Unterscheidung zwischen administrativen Nutzern und genehmigten Tutoren.
- Die Verwaltung von Fächern, Klassen und Nachhilfegesuchen.
- Hashing von Benutzerpasswörtern mit einem zusätzlichen "Pepper".

## Features

- **Login und Registrierung:**  
  Überprüft in der Login-Funktion, ob ein Benutzer entweder Administrator ist oder als Tutor genehmigt wurde.

- **Fach-, Klassen- und Bildungsgangverwaltung:**  
  Erlaubt die Erstellung, Löschung und Abfrage von Fächern und Klassen in der Datenbank.

- **Zeitspannenverwaltung:**  
  Verwaltung und Überprüfung von Zeitspannen, in denen Schüler oder Tutoren verfügbar sind.

- **Sicherheitsmechanismen:**  
  Passwörter werden mit SHA-256 zusammen mit einem festen "Pepper" gehashed, um die Sicherheit zu erhöhen.

## Storyboard
[Storyboard](https://terra.ipmake.me/s/7J28WbQxT6K6yry)

## UseCase
[UseCase](https://terra.ipmake.me/s/jDGFk5n77fx2kEg)

