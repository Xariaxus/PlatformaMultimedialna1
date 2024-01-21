# PlatformaMultimedialna
Działanie aplikacji

1 Uruchomienie

Gdy uruchomisz aplikację to ukaże ci się strona główna. Na górnym pasku znajduje się od lewej strony „Strona główna”, „Polityka prywatności”, 
„Twoje media”, „Rejestracja”, „Logowanie”.

UWAGA! ZANIM URUCHOMISZ APLIKACJE, NAJPIERW ZRÓB MIGRACJĘ.
Narzędzia -> Menedżer pakietów NuGet -> Konsola menedżera pakietów.
(Wykonaj poniższe komendy)
Add-Migration NazwaMigracji 
Update-Database
„NazwaMigracji” – może być dowolna.

2 Twoje media
Aby przejść na stronę „Twoje media” użytkownik musi być zalogowany.
Jeżeli nie jesteś zalogowany to strona automatycznie przekieruje cię na stronę logowania.

3 W „Twoje media”
Po udanym zalogowaniu się, użytkownik może przejść na stronę „Twoje media”, gdzie może wrzucić swoje pliki z rozszerzeniem „.jpg”, „.png”, „.jpeg”, „.mp3” oraz „.mp4”. Jeżeli wrzucisz plik na stronę to automatycznie dane pliku dodają się do bazy danych „dbo.Media” i z tej bazy dane są pobierane i serializowane do pliku tekstowego dane_z_bazy.txt  w „wwwroot”.





UWAGA.
Projekt współgra z przeglądarkami takimi jak. „Google Chrome”, „Opera”, „Mozilla Firefox”, “Microsoft Edge”. 
Nie uruchamiać na przeglądarce “Brave”.

Nie wrzucać plików z rozszerzeniem .mp4 I mp.3 powyżej 25mb ponieważ nie wrzuci się na stronę. Powodem może być ograniczenie ze strony środowiska.

Problemem z którym się mierzyliśmy lecz nie udało nam się go rozwiązać to:
- Gdy dodacie na stronę plik z rozszerzeniem .mp3 i .mp4 to nie wyświetli wam się obraz tylko napis „Obraz” ale nadal działa jego pobranie i usuwanie oraz wyświetla się jego nazwa. 

- Również pliki „.mp3” i „.mp4” nie dodają się do bazy danych do rekordu. 



Jakub Girek oraz Mateusz Szymański gr.K13
