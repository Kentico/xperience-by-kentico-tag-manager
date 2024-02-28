1. CodeSnippetTypesDropdownOptionsProvider
    1. Bral by data z nejakej metody nejakej inej classy - ta moze ale spis nemusi mat dependency injection
    2. Text a value by mali vychadzat z nejakeho objektu kt. bud bude mat nejaky polymorfizmus alebo bude mat nejaku metodu, kt. nadefinuje ze ci sa ma pouzit script - potom ze co sa ma stat s tym danym id - co ma pridavat ten dany tag do stranok a ake api volat
2. CodeSnippetTypes to nemoze byt - chceme pracovat s classami - nevieme ake budu  typy
3. GetConsentedCodeSnippets - GetCodeSnippetsInternal - CreateCodeSnippet - vybera data z databazy - na zaklade nich vytvori dto, kt ma id, kod, lokaciu - to je spravne, ale kazdy typ tu ma static metodu, ze co ma vygenerovat - toto chceme dat do toho noveho objektu

4. Navrh - tag option factory kt. bude mat metodu create dto - dostane hlavne to id a vrati dto s scriptom, kt. si ma clovek napisat sam - ja niektore - je to virtual factory. 
   1. vytvori dto
   2. options
   3. meno toho tagu
   4. text do option provideru
   5. value do option provideru