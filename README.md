# LernBuddy - Müther und Meyer

## Inhaltsverzeichnis

- [Features](#features)
  - [Umgesetzte Funktionen](#umgesetzte-funktionen)
  - [Noch zu implementierende Funktionen](#noch-zu-implementierende-funktionen)
- [UseCase](#usecase)
- [StoryBoard](#storyboard)

## Features

### Umgesetzte Funktionen

- **Dashboard:**
  - Anzeige aller Nachhilfegesuche in einer Liste, sortiert nach Datum.
  - Interaktive Steuerung mit Filtermöglichkeiten nach Fach und Bildungsgang.
  - Suchfunktion zur schnellen Auffindung von Gesuchen anhand von Schlüsselwörtern.

- **Benutzerrollen & Funktionen:**

  - **Nachhilfesuchende (Schüler:innen, die Hilfe benötigen):**
    - Anonymes Erstellen von Nachhilfegesuchen über ein Formular mit folgenden Eingabefeldern:
      - Vorname, Nachname, Geschlecht, E-Mail
      - Klasse, Bildungsgang
      - Fach
      - Freitextfeld für zusätzliche Informationen
      - Mögliche Termine
    - Nach Absenden erscheint die Anfrage anonymisiert im Dashboard.

  - **Nachhilfegebende (Tutoren):**
    - Registrierung mit folgenden Daten:
      - Vorname, Nachname, Geschlecht, E-Mail
      - Klasse, Bildungsgang
      - Passwort für den Login
    - Login-Funktion mit E-Mail und Passwort.
    - Zugriff auf alle offenen Nachhilfegesuche mit Möglichkeit zur Einsicht der Kontaktdaten der Nachhilfesuchenden.
    - Option, ein Gesuch als "angenommen" zu markieren, wodurch es aus der Liste entfernt oder entsprechend gekennzeichnet wird.

  - **Administratoren (Admins):**
    - Prüfung und Genehmigung von Nachhilfelehrern.
    - Löschung missbräuchlicher Einträge.
    - Sperrung von Schüler:innen, die gegen Regeln verstoßen.
    - Manuelles Entfernen alter oder überflüssiger Nachhilfegesuche.

### Noch zu implementierende Funktionen

- **Erweiterte Filter- und Sortierfunktionen:**
  - Fehlt komplett

- **Erweiterte Benutzerinteraktionen:**
  - Funktionen wie das direkte Senden von Nachrichten zwischen Nachhilfesuchenden und Tutoren oder die Möglichkeit für Tutoren.
- **Administrative Zusatzfunktionen:**
  - Funktionen wie das Sperren von Schüler:innen, die gegen Regeln verstoßen, oder das manuelle Entfernen alter Nachhilfegesuche fehlen.


# UseCase
[Usecase](https://terra.ipmake.me/s/jDGFk5n77fx2kEg)
# StoryBoard
[StoryBoard](https://terra.ipmake.me/s/Mynw4Zd5fEct8J7)
