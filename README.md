

# LABB3 - GUI
Inlämningsuppgift i kursen Programmering med C#

### Quiz Configurator
Uppgiften går ut på att skapa en applikation för att kunna konfigurera quiz-frågor, och köra quiz-rundor.
Appen har skrivits i WPF, XAML, C# och är uppbyggd enligt Model-View-ViewModel (MVVM)-arkitektur.

### Några tekniska lösningar
En av de tekniska lösningar jag har använt mig av är Fisher-Yates shuffle-algoritmen för blandning av indexen på mina svarsalternativ. 
Här lägger jag först alla mina svarsalternativ i en lista där varje alternativ gås igenom med start bakifrån. För varje svar sparas detta först i en temporär variabel, därefter slumpas det fram ett nytt index inom räckvidden och de två alternativen byter sedan plats med varandra. 
Jag valde denna lösning eftersom den ger en likformig slumpfördelning och inte behöver skapa nya tillfälliga listor under tiden. 

För att byta vyer i mitt program har jag använt mig av en ContentControl i MainWindow.xaml som jag har knutit till propertyn CurrentView i MainWindowViewModel.  
Med hjälp av två DataTemplate-mallar, där respektive DataType representeras av antingen PlayerViewModel eller ConfigurationViewModel, avgörs det vilken UserControl som ska visas beroende på vilket objekt som ligger i CurrentView. 

### Ett av de problemen som uppstod
Ett av problemen jag stötte på var vid deserialiseringen av sparade frågor från JSON. 
När programmet startades om var ListBoxen för mina frågor helt tom men i AppData-mappen kunde jag se att JSON-filen hade sparats ner som den ska. 
Felet låg alltså i själva inläsningen där JSON-filens string-array (IncorrectAnswers) inte matchade med Question-klassens konstruktor som tar emot tre separata string-parametrar. 

Först löste jag problemet genom att lägga till en tom konstruktor, men lösningen kändes dock inte helt optimal då det säkerligen kan dyka upp problem om JSON får välja helt fritt hur datan ska deserialiseras och att nödvändig information riskerar att utelämnas. 
Istället la jag till ytterligare en konstruktor, märkt med [JsonConstructor]. 
Denna tar emot de felaktiga svaren direkt som en string-array som matchas direkt mot egenskapen IncorrectAnswers, samtidigt som jag kunde behålla min ursprungliga konstruktor som den är för resten av programmet. 