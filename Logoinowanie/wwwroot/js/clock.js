// JavaScript source code
	function odliczanie()
	{
		var dzisiaj = new Date(); //nowy obiekt Daty
		
		var godzina = dzisiaj.getHours();
		if (godzina<10) godzina = "0"+godzina;//dodawanie 0 do godziny jeœli mniej ni¿ 10
		
		var minuta = dzisiaj.getMinutes();
		if (minuta<10) minuta = "0"+minuta;//dodawanie 0 do minuty jeœli mniej ni¿ 10
		
		var sekunda = dzisiaj.getSeconds();
		if (sekunda<10) sekunda = "0"+sekunda;//dodawanie 0 do sekundy jeœli mniej ni¿ 10		
		
		//migaj¹cy : co 1s
		document.getElementById("zegar").innerHTML = godzina + ":" + minuta + ":" + sekunda;
		setTimeout(function tic_tac(){
			document.getElementById("zegar").innerHTML = godzina + " " + minuta + " " + sekunda;
		}, 500);
			
		 setTimeout("odliczanie()",1000);
	}
	
$(document).ready(function(){
	odliczanie();
});