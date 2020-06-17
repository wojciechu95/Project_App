using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logoinowanie.Entities
{
    public class DatabaseManager
    {

        public DatabaseManager()
        {

        }

        static public void GetConnection()
        {
            /*$conn = new mysqli(DB_SERVER, DB_USERNAME, DB_PW, DB_DB); //połączenie, podajemy stałe zdefiniowane w config.php

            if (mysqli_connect_errno())
            { //wystąpił błąd w połączeniu
            $conn_errno = mysqli_connect_errno();
            $conn_error = mysqli_connect_error();
                echo $conn_errno."<br />".$conn_error; //wyświetlamy komunikat o błędzie
                return false;

            }
            else
            { //połączenie udane
            $conn->set_charset("utf8");//ustawianie kodowania połączenia
                return $conn; //zwracamy połączenie

            }*/
        }

        static public void SelectBySQL()
        { //zapytanie sql w parametrze

        /*$conn = self::getConnection(); //łączymy z bazą danych
        $result = $conn->query($SQL); //wykonujemy zapytanie


            if (!$result) { //zapytanie nie powiodło się

                return false; //zwracamy false

            } else
            {

            $resultArray = Array(); //tworzymy tablicę

                while (($row = $result->fetch_array(MYSQLI_ASSOC)) !== NULL) { //dodajemy wyniki zapytania do tablicy

                $resultArray[] = $row;
                }

            }

            if (count($resultArray) > 0)
            {
                return $resultArray; //Jeśli tablica zawiera wyniki, czyli odnaleziono dane w bazie danych to zwracamy tą tablicę z wynikami
            }
            else
            {
                return false; //Zwracamy false
            }

            mysqli_close($conn); //zamknięcie połączenia*/

        }

        static public void UpdateTable(/*$TABLE, $SET, $WHERE = Array(), $OPER = "AND"*/)
        { // wzór zapytania: "UPDATE tabela SET kolumna='wartosc' WHERE kolumna='wartosc'"

        /*$conn = self::getConnection(); //łączymy z bazą danych

        $SQL = "UPDATE {$TABLE} SET "; //początek zapytania

            foreach ($SET as $key => $val) { //pętla przechodząca przed tablicę $SET
            $SQL.= $key."='".$val."',"; //dodawanie do zapytania wartości do ustawienia
            }

        $SQL = rtrim($SQL, ','); //obcięcie ostatniego przecinka z zapytania

            if (count($WHERE) > 0)
            { //sprawdzenie czy tablica $WHERE ma zawartość

            $SQL.= " WHERE "; //jeśli ma dodajemy do zapytania WHERE

                foreach ($WHERE as $key => $val) { //analogicznie jak przy $SET
                $SQL.= $key."='".$val."' ".$OPER." "; //uzupełniamy zapytanie o warunki odzielone operatorem (domyślnie AND)
                }

            $SQL = substr($SQL, 0, strlen($SQL) - (strlen($OPER) + 2)); //obcięcie końcowego operatora

            }

        $result = $conn->query($SQL); //wystosowanie zapytania

            if ($result) {
                return true; //powodzenie, zwracamy true
            } else
            {
                return false; //niepowodzenie, zwracamy false
            }

            mysqli_close($conn); //zamknięcie połączenia*/

        }

        static public void DeleteFrom(/*$TABLE, $WHERE = Array(), $OPER = "AND"*/)
        { // wzór zapytania "DELETE FROM tabela WHERE kolumna='wartosc'"

        /*$conn = self::getConnection(); //połączenie z bazą

        $SQL = "DELETE FROM {$TABLE}"; //początek zapytania

            if (count($WHERE) > 0)
            { //jeśli tabela $WHERE posiada zawartość

            $SQL.= " WHERE "; //dodajemy do zapytania WHERE

                foreach ($WHERE as $key => $val) { //przechodzimy przez tablicę w pętli

                $SQL.= $key."='".$val."' ".$OPER." "; //dodajemy warunki do zapytania

                }

            $SQL = substr($SQL, 0, strlen($SQL) - (strlen($OPER) + 2)); //usuwamy końcowy operator

            }

        $result = $conn->query($SQL); // wykonujemy zapytanie
            if (!($result)) {
                return false; //zapytanie nieudane, zwracamy false
            } else
            {
                return true; //zapytanie udane, zwracamy true
            }

            mysqli_close($conn); //zamknięcie połączenia*/

        }

        static public void InsertInto(/*$TABLE, $DATA*/)
        { //wzór zapytania "INSERT INTO table_name (column1, ...) VALUES (value1, ...)"

      /*  $conn = self::getConnection(); //połączenie z bazą

        $SQL = "INSERT INTO {$TABLE}"; //początek zapytania
        $SQL.= " (";

            foreach ($DATA as $key => $val) {
            $SQL.= $key.","; //dodanie do zapytania wybranych kolumn
            }

        $SQL = rtrim($SQL, ","); //obcięcie przecinka z końca zapytania
        $SQL.= ") ";
        $SQL.= "VALUES";
        $SQL.= " (";

            foreach ($DATA as $val) {
            $SQL.= "'".$val."',"; //dodanie do zapytania wybranych wartości
            }

        $SQL = rtrim($SQL, ','); //obcięcie przecinka z końca zapytania
        $SQL.= ")";

        $result = $conn->query($SQL); //wystasowanie zapytania
            if (!($result)) {
                return false; //niepowodzenie, zwracamy false
            } else
            {
                return true; //powodzenie, zwracamu true
            }

            mysqli_close($conn); //zamknięcie połączenie*/

        }
    }
}