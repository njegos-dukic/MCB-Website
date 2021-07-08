use mastercraftbrewery;
-- Company information
insert into company(name, address, postalCode, coordinates, email, fax, apiKey) 
values ('The Master Craft Brewery', 'Bulevar Vojvode Stepe Stepanovica 44, Banja Luka', '78000',
'44.7638541555097, 17.1978036690856', 'info@themastercraftbrewery.com', null, '0f8fad5b-d9cb-469f-a165-70867728950e');
set @companyId = last_insert_id();

-- Company phone
insert into phone(phoneNumber,companyId) values ('0038765626110', @companyId);
insert into phone(phoneNumber,companyId) values ('0038765916917', @companyId);

-- Company wholesale
insert into wholesale(name, address, coordinates, companyId)
values ('Veliki Tropic','Ivana Gorana Kovačevića bb','44.78975817670075, 17.203742234428717', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Moj market','Starčevica, Majke Jugovića 23b','44.76326415202695, 17.201273103748754', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Moj market','Obilićevo, Cara Lazara 23b','44.762879393246784, 17.191083499999998', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Moj market','Svetozara Markovića 14','44.778260957049056, 17.193077000464665', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Moj market','Borik, Bulevar Živojina Mišića 10','44.77109646420218, 17.204643467924477', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Moj market','TC Delta','44.77229532705265, 17.191289677184542', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Moj market','TC Boska','44.77011153137257, 17.189739786972474', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Market AS','Starčevica, Slobodana Kusturića 17','44.76405349048689, 17.203040873480376', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Market AS','Starčevica, Jug Bogdana bb','44.76188669095534, 17.201304798612306', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Market AS','Nova Varoš, Svetozara Markovića 14','44.77636099255461, 17.193340509988573', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Market AS','Lauš, Užička 2a','44.7787223986482, 17.16832457348064', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Market AS','Paprikovac, Omladinska bb','44.7632684109418, 17.175287413956394', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Frutela Group','Wonderland poklon shop, Srpska 99','44.77039341476979, 17.197940123216544', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Frutela Group','Kuća pića, Branka Popovića 41b','44.810072890213604, 17.210008239710078', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Zoki komerc','Kneza Miloša 48','44.79924179970194, 17.208452211577026', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Hiperkort','Derviši, Branka Popovića 310','44.81810772589622, 17.216365402845337', @companyId);
insert into wholesale(name, address, coordinates, companyId)
values ('Hiperkort','Malta, dr Mladena Stojanovića bb','44.783889249145034, 17.202928223216755', @companyId);

-- Company social medias
set @facebook = 0;
set @instagram = 3;
insert into socialMedia(url, type, companyId) values ('https://www.facebook.com/themastercraftbrewery', @facebook, @companyId);
insert into socialMedia(url, type, companyId) values ('https://www.instagram.com/the_master_craft_brewery/', @instagram, @companyId);

-- Root administrator
insert into administrator(email, salt, password, companyId) 
values ('root.administrator@gmail.com',cast(unhex('E341EB1D56B2E542A0296B2D2E4D89DC') as binary),
cast(unhex('2815AAC3094980EDB836763D7EB84C3A48DAFF36F990BF2AC1D9F837A6DAE1DB') as binary),@companyId);

-- Product types
insert into productType(name) values('Pivo');
set @beerId= last_insert_id();
insert into productType(name) values('Hrana');
set @foodId = last_insert_id();
insert into productType(name) values ('Koktel');
set @coctailId = last_insert_id();

-- Shop amount
insert into ShopAmount(packageAmount, incrementAmount, companyId) 
values(20, 4, @companyId);
set @smallerPackage = last_insert_id();
insert into ShopAmount(packageAmount, incrementAmount, companyId) 
values(6, 1, @companyId);
set @hugePackage = last_insert_id();

-- Servings
insert into serving(name) values('Mala');
insert into serving(name) values('Velika');
insert into serving(name) values('Standardna');
set @classic = last_insert_id();
insert into serving(name) values('0.33l');
set @bottle = last_insert_id();
insert into serving(name) values('Plata');
set @hugePlate = last_insert_id();
insert into serving(name) values ('0.5l');
insert into serving(name) values('1l');
insert into serving(name) values('2l');
set @hugeBottle = last_insert_id();

-- Food menu
insert into menu(name, description, companyId)
values ('Glavni jelovnik', 'Moguće je rezervisati i organizovati razne događaje (poslovne sastanke, rođendane, proslave i slično). Imamo happy hours od 15-18h. Uz poručen burger ili rebarca, 
dobićete MCB pivo 0.33l za 1KM.', @companyId);
set @mainMenu = last_insert_id();
insert into menu(name, description, companyId)
values ('Doručak', 'Započnite dan uz kvalitetan MCB doručak. Moguće je rezervisati i organizovati razne događaje (poslovne sastanke, rođendane, proslave i slično). Imamo happy hours od 15-18h. Uz poručen burger ili rebarca, dobićete MCB pivo 0.33l za 1KM.',
 @companyId);
set @breakfast = last_insert_id();
insert into menu(name, description, companyId)
values ('Burgeri', 'Uživajte u fantastičnim, sočnim MCB burgerima. Otkrijte svoj ukus. Moguće je rezervisati i organizovati razne događaje (poslovne sastanke, rođendane, proslave i slično). Imamo happy hours od 15-18h. Uz poručen burger ili rebarca, 
dobićete MCB pivo 0.33l za 1KM.', @companyId);
set @burgers = last_insert_id();

-- Products
-- Main food menu
insert into product(name, description, photoUri, companyId, productTypeId)
values('Master plata za 4 osobe','Ako želite da počastite sebe i društvo, odabrali ste pravo jelo. Različite vrste mesa će aktivirati sva vaša čula.', 'images\\products\\master_plata.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @hugePlate, 24.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Pileća krilca');
	insert into ingredient(productId, name) values (@productId, 'Svinjska kobasica');
    insert into ingredient(productId, name) values (@productId, 'Svinjska rebarca');
    insert into ingredient(productId, name) values (@productId, 'MCB BBQ sos');

insert into product(name, description, photoUri, companyId, productTypeId)
values('Plata tri kobasice', 'Sočne i savršene, za prave gurmane i ljubitelje piva.', 'defaultProductImage.png', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @hugePlate, 22.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Svinjska kobasica');
	insert into ingredient(productId, name) values (@productId, 'Pomfrit');
    insert into ingredient(productId, name) values (@productId, 'MCB BBQ sos');

insert into product(name, description, photoUri, companyId, productTypeId)
values('Koljenica sa kremom od hrena', 'Ovakvu koljenicu će obožavati pravi ljubitelji mesa i ljutog. Hren doprinosi savršenoj aromi, osvojiće Vas na prvi zalogaj.', 'images\\products\\koljenica.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 35.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Svinjska koljenica');
	insert into ingredient(productId, name) values (@productId, 'Hren');
    insert into ingredient(productId, name) values (@productId, 'Pomfrit');

insert into product(name, description, photoUri, companyId, productTypeId)
values('Dimljeni pileći batak', 'Mekan i sočan, sa ukusom dima, ide savršeno uz sve vrste piva.', 'images\\products\\dimljeni_batak.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 15.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Dimljeni pileći batak');
	insert into ingredient(productId, name) values (@productId, 'Pomfrit');
    insert into ingredient(productId, name) values (@productId, 'Domaći umak od gorušice');

insert into product(name, description, photoUri, companyId, productTypeId)
values('Rebarca na pekarskom krompiru u sosu od kamenica', 'Želite ukus mora na svom tanjiru, ali ste vjerni klempi? Svinjska rebarca u sosu od kamenica zadovoljavaju kriterijume i najvećih gurmana.', 'images\\products\\rebarca_sos_od_kamenica.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 15.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Svinjska rebarca');
	insert into ingredient(productId, name) values (@productId, 'Domaći umak od kamenica');
    insert into ingredient(productId, name) values (@productId, 'Krompir');
    insert into ingredient(productId, name) values (@productId, 'Začini');

insert into product(name, description, photoUri, companyId, productTypeId)
values('Pileći filet sa njokama u pikant sosu', 'Tradicionalni njoki, toliko dobri da ćete tražiti još jednu porciju. Toplo, blago pikantno i nježno. Probudiće gurmana u Vama.', 'images\\products\\pileci_filet_u_sosu.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 13.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Pileći filet');
	insert into ingredient(productId, name) values (@productId, 'Pavlaka');
    insert into ingredient(productId, name) values (@productId, 'Ljuta paprika');
    insert into ingredient(productId, name) values (@productId, 'Curry');
    insert into ingredient(productId, name) values (@productId, 'Domaći njoki od krompira i jaja');
    insert into ingredient(productId, name) values (@productId, 'Ostali začini');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Pivske kobasice sa sirom urolane u slaninu', 'Šta reći nego fino, fino, fino. Hrskava slaninica, sočne kobasice sa blagim ukusom sira neće ostaviti nikog ravnodušnim.', 'images\\products\\pivske_kobasice_u_slanini.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 20.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Pivska kobasica');
	insert into ingredient(productId, name) values (@productId, 'Umak od cheddara');
    insert into ingredient(productId, name) values (@productId, 'Slanina');
    insert into ingredient(productId, name) values (@productId, 'Pomfrit');
    insert into ingredient(productId, name) values (@productId, 'Začini');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Pileća krilca u slatko-ljutom sosu', 'MCB BBQ sos dolazi do izražaja upravo uz vruća pileća krilca. Pivo, krilca, dobro društvo..','images\\products\\pileca_krilca_u_sosu.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 15.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Pileća krilca');
	insert into ingredient(productId, name) values (@productId, 'MCB BBQ sos');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Domaće kobasice sa sirom i marmeladom od luka', 'Neopisiva sočnost, uz savršeno nježnu marmeladu od luka... Za ljubitelje domaće hrane i domaćeg piva.', 'images\\products\\kobasice_sir_luk.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 10.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Domaća svinjska kobasica');
	insert into ingredient(productId, name) values (@productId, 'Sir');
    insert into ingredient(productId, name) values (@productId, 'Luk');
    insert into ingredient(productId, name) values (@productId, 'Pomfrit');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Hrskavi pileći štapići', 'Hrskavi pileći štapići su idealan dodatak uz čašu vašeg omiljenog MCB craft piva, idu uz apsolutno sve😉🍺🍻🌯. Klasično, jednostavno, a opet zahtjeva vještinu prženja da bi se postigla nenadmašna hrskavost i zadržala sočnost mesa. Uvjerite se da su naši štapići najbolji izbor.', 'images\\products\\pileci_stapici.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 10.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Pileći filet');
	insert into ingredient(productId, name) values (@productId, 'Susam');
    insert into ingredient(productId, name) values (@productId, 'Krušne mrvice');
	insert into ingredient(productId, name) values (@productId, 'Umak od vrhnje');
    insert into ingredient(productId, name) values (@productId, 'Pomfrit');
    insert into ingredient(productId, name) values (@productId, 'Začini');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Pikantne kobasice sa sosom gorušice', 'Srednja ljutina, idealna za ljubitelje senfa, podiže ukus kobasice na novi nivo. Umak od gorušice je jednostavno savršen sa svim vrstama roštilja, a za Vas su odabrane sočne kobasice. ,', 'images\\products\\ljute_kobasice.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 15.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Ljuta svinjska kobasica');
	insert into ingredient(productId, name) values (@productId, 'Domaći sos od gorušice');
    insert into ingredient(productId, name) values (@productId, 'Pomfrit');
    insert into ingredient(productId, name) values (@productId, 'Začini');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Vegeterijanska meksička mješavina', 'Meksička kuhinja privlači brojne gurmane sveta zbog svojih pikantnih ukusa u kojima caruju namirnice poput kukuruza, graška i čilija. Jeste li znali zašto je meksička kuhinja ljuta? Jedan od razloga je kako bi ljuti začini ubili sve neželjene bakterije, a drugi kako bi na što intenzivniji način doživeli čari jela.', 'defaultProductImage.png', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 8.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Kukuruz');
	insert into ingredient(productId, name) values (@productId, 'Crvena paprika');
    insert into ingredient(productId, name) values (@productId, 'Grah');
    insert into ingredient(productId, name) values (@productId, 'Začini');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Burger Master Smith sa cheddar sirom i pomfritom', 'Roštilj, druženje i filantropija, savršena trojka!! Master Smith osvojiće titulu najboljeg burgera u Vašem životu.', 'images\\products\\burger_smith.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 9.00);
insert into menuItem(menuId, productServingId) values(@burgers, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Svinjsko mljeveno meso');
	insert into ingredient(productId, name) values (@productId, 'Cheddar sir');
    insert into ingredient(productId, name) values (@productId, 'Zemičke od pšeničnog brašna');
    insert into ingredient(productId, name) values (@productId, 'Pomfrit');
	insert into ingredient(productId, name) values (@productId, 'Zelena salata');
    insert into ingredient(productId, name) values (@productId, 'Začini');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Pomfrit aromatizovan peperončinima', 'Peperončini nisu dobar izbor samo na pizzi. Probajte hrskav i vruć pomfrit, savršen uz MCB pivo.', 'defaultProductImage.png', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 8.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Pomfrit');
	insert into ingredient(productId, name) values (@productId, 'Peperončini');
    insert into ingredient(productId, name) values (@productId, 'Začini');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Burger sa junećim mesom', ' Klasičan, a opet kralj roštilja je burger. Mekano tijesto, sočno meso, intenzivni prilozi.', 'images\\products\\burger_junece_meso.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 9.00);
insert into menuItem(menuId, productServingId) values(@burgers, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Juneće mljeveno meso');
	insert into ingredient(productId, name) values (@productId, 'Zemičke od pšeničnog brašna');
    insert into ingredient(productId, name) values (@productId, 'Paradajz');
    insert into ingredient(productId, name) values (@productId, 'Zelena salata');
    insert into ingredient(productId, name) values (@productId, 'Začini');
	
insert into product(name, description, photoUri, companyId, productTypeId)
values('Burger Smoked Rooster', ' Kralj roštilja je burger. Mekano tijesto, sočno upajcano meso dimljenog pilećeg bataka, nježni prilozi.', 'images\\products\\smoked_rooster.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 9.00);
insert into menuItem(menuId, productServingId) values(@burgers, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Dimljeni pileći batak');
	insert into ingredient(productId, name) values (@productId, 'Zemičke od pšeničnog brašna');
	insert into ingredient(productId, name) values (@productId, 'Tartar sos');
	insert into ingredient(productId, name) values (@productId, 'Pomfrit');
    insert into ingredient(productId, name) values (@productId, 'Paradajz');
    insert into ingredient(productId, name) values (@productId, 'Zelena salata');
    insert into ingredient(productId, name) values (@productId, 'Začini');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('MCB burger', ' Čist hedonizam: MCB burger sa sosom od kamenica. Mekano tijesto, sočno meso, intenzivni prilozi i umak sa domaćim potkozarskim medom. 🙂👌🍔🍺', 'images\\products\\mcb_burger.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 9.00);
insert into menuItem(menuId, productServingId) values(@burgers, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Juneće mljeveno meso');
	insert into ingredient(productId, name) values (@productId, 'Zemičke od pšeničnog brašna');
	insert into ingredient(productId, name) values (@productId, 'Tartar sos');
	insert into ingredient(productId, name) values (@productId, 'Pomfrit');
	insert into ingredient(productId, name) values (@productId, 'Med');
	insert into ingredient(productId, name) values (@productId, 'Soja sos');
    insert into ingredient(productId, name) values (@productId, 'Sos od kamenica');
    insert into ingredient(productId, name) values (@productId, 'Cheddar');
    insert into ingredient(productId, name) values (@productId, 'Začini');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Pekarski krompir', 'Starinski, jednostavan, domaći krompir. Potrebno je imati par vještina da bi svaki put bio ukusan, a naši kuvari ih posjeduju. Uživajte. ', 'defaultProductImage.png', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @classic, 8.00);
insert into menuItem(menuId, productServingId) values(@mainMenu, last_insert_id());
	-- Ingredients
    insert into ingredient(productId, name) values (@productId, 'Krompir');
    insert into ingredient(productId, name) values (@productId, 'Začini');
    
-- Breakfast food menu
insert into product(name, description, photoUri, companyId, productTypeId)
values('Omlet sa šunkom', 'Jednostavno do kvalitetnog doručka. Sve što vam treba za dobar početak dana. Ljubitelji šunke, uživajte.', 'defaultProductImage.png', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @hugePlate, 5.00);
insert into menuItem(menuId, productServingId) values(@breakfast, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Kokošija jaja');
	insert into ingredient(productId, name) values (@productId, 'Svinjska šunka');
    insert into ingredient(productId, name) values (@productId, 'Sunokretovo lje');
    insert into ingredient(productId, name) values (@productId, 'So');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Omlet sa piletinom','Proteinska bomba, savršeno za naporan dan, osigurajte svom tijelu kvalitetan obrok. Ipak je doručak najvažniji obrok u danu..', 'defaultProductImage.png', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @hugePlate, 7.00);
insert into menuItem(menuId, productServingId) values(@breakfast, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Kokošija jaja');
	insert into ingredient(productId, name) values (@productId, 'Pileći filet');
    insert into ingredient(productId, name) values (@productId, 'Suncokretovo ulje');
    insert into ingredient(productId, name) values (@productId, 'So');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Omlet sa cheddar sirom', 'Mliječna aroma cheddar sira, jedinstvena topivost, prepustite se čarima dobrog doručka.', 'defaultProductImage.png', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @hugePlate, 5.00);
insert into menuItem(menuId, productServingId) values(@breakfast, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Kokošija jaja');
	insert into ingredient(productId, name) values (@productId, 'Cheddar sir');
    insert into ingredient(productId, name) values (@productId, 'Suncokretovo ulje');
    insert into ingredient(productId, name) values (@productId, 'So');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Omlet sa piletinom i cheddar sirom', 'Proteinska bomba, savršeno za naporan dan, osigurajte svom tijelu kvalitetan obrok. Ipak je doručak najvažniji obrok u danu..', 'defaultProductImage.png', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @hugePlate, 8.00);
insert into menuItem(menuId, productServingId) values(@breakfast, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Kokošija jaja');
	insert into ingredient(productId, name) values (@productId, 'Cheddar sir');
    insert into ingredient(productId, name) values (@productId, 'Pileći filet');
    insert into ingredient(productId, name) values (@productId, 'Suncokretovo ulje');
    insert into ingredient(productId, name) values (@productId, 'So');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Omlet sa šunkom i cheddar sirom', 'Omlet sa sirom i šunkom je izuzetno ukusan, hranljiv i prepun proteina, što ga čini idealnim doručkom bez obzira na uzrast', 'defaultProductImage.png', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @hugePlate, 7.00);
insert into menuItem(menuId, productServingId) values(@breakfast, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Kokošija jaja');
	insert into ingredient(productId, name) values (@productId, 'Cheddar sir');
    insert into ingredient(productId, name) values (@productId, 'Svinjska šunka');
    insert into ingredient(productId, name) values (@productId, 'Suncokretovo ulje');
    insert into ingredient(productId, name) values (@productId, 'So');
    
insert into product(name, description, photoUri, companyId, productTypeId)
values('Master doručak', 'Klasični doručak za zahtjevnije mušterije, napuniće Vas energijom dovoljnom da osvojite svijet. Prepustite se domaćoj aromi jaja i kobasica.', 'images\\products\\master_dorucak.jpg', @companyId, @foodId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @hugePlate, 10.00);
insert into menuItem(menuId, productServingId) values(@breakfast, last_insert_id());
	-- Ingredients
	insert into ingredient(productId, name) values (@productId, 'Domaća kobasica');
	insert into ingredient(productId, name) values (@productId, 'Pavlaka');
    insert into ingredient(productId, name) values (@productId, 'Jaja');
    insert into ingredient(productId, name) values (@productId, 'Paradajz');
    insert into ingredient(productId, name) values (@productId, 'So');
    
-- Beers
insert into product(hexColor, name, description, photoUri, companyId, productTypeId)
values('#338ec7','Banjalucki Kraft',
'MCB Banjalučki kraft. Naš poklon Banjaluci, session ale u koji smo ugradili mnogo ljubavi i pažnje ❤. 
MCB Banjalučki kraft je lagano i pitko, zanatski radjeno pivo i pravi je uvod u svijet kraft piva. 
Laganim tijelom i dobrom gaziranošću idealno je rješenje za hidrataciju i svaki put kad poželimo uživati uz nekoliko piva.',
'images\\products\\BanjaluckiKraft.jpg',
@companyId, @beerId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @bottle, 5.00);
insert into productserving(productId, servingId, price)
values(@productId, @hugeBottle, 8.00);
-- Online shop
insert into shopproductserving(productId, servingId, price, photoUri, shopAmountId)
values(@productId, @bottle, 5.00,'images\\products\\BanjaluckiKraft.jpg',@smallerPackage);
insert into shopproductserving(productId, servingId, price, photoUri, shopAmountId)
values(@productId, @hugeBottle, 8.00,'images\\products\\kraft2l.png',@hugePackage);

insert into product(hexColor, name, description, photoUri, companyId, productTypeId)
values('#0f0f0f', 'Stout',
'MCB Stout je tamno pivo bogatog ukusa sa kompleksnim aromama čokolade, kafe i prženog slada. Hmeljnost je u pozadini prisutna sa laganom cvjetnom aromom a u ukusu preovladava lagana prženost u kojoj se miješaju kafa i čokolada. 
MCB Stout je srednje punog tijela sa slatkoćom koja dolazi od dodate laktoze tokom kuvanja. Lagane je gaziranosti i odličan je kako za zimske tako i za tople ljetne dane. 
Kraft pivo koje se odlično slaže i sa desertima i sa mesnim delicijama.',
'images\\products\\Stout.jpg',
@companyId, @beerId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @bottle, 5.00);
insert into productserving(productId, servingId, price)
values(@productId, @hugeBottle, 8.00);
-- Online shop
insert into shopproductserving(productId, servingId, price, photoUri, shopAmountId)
values(@productId, @bottle, 5.00,'images\\products\\Stout.jpg', @smallerPackage);
insert into shopproductserving(productId, servingId, price, photoUri, shopAmountId)
values(@productId, @hugeBottle, 8.00,'images\\products\\stout2l.png', @hugePackage);

insert into product(hexColor, name, description, photoUri, companyId, productTypeId)
values('#ce7801', 'Pale Ale',
'Pažljivo biranom kombinacijom njemačkih sladova i američkih sorti hmelja stvorili smo ovaj MCB Pale Ale bogatog ukusa i naročite pitkosti koji će vas oduševljavati svakim gutljajem. 
MCB Pale Ale je srednje laganog tijela i gaziranosti u čijem je ukusu prisutna lagana slatkoća i umjerena hmeljna gorčina. Dominiraju arome grejpa i mandarine sa tragovima borovine.',
'images\\products\\PaleAle.jpg',
@companyId, @beerId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @bottle, 5.00);
insert into productserving(productId, servingId, price)
values(@productId, @hugeBottle, 8.00);
-- Online shop
insert into shopproductserving(productId, servingId, price, photoUri, shopAmountId)
values(@productId, @bottle, 5.00,'images\\products\\PaleAle.jpg', @smallerPackage);
insert into shopproductserving(productId, servingId, price, photoUri, shopAmountId)
values(@productId, @hugeBottle, 8.00,'images\\products\\paleAle2l.png', @hugePackage);

insert into product(hexColor, name, description, photoUri, companyId, productTypeId)
values('#e4b503', 'Pilsner',
'MCB Pilsner je zlatno žute boje sa bogatom bijelom pjenom. Ovo pivo je naš odgovor na industrijski lager. 
MCB Pilsner je bogatog ukusa sa laganim tijelom i umjerenom gaziranošću. Ukus je lagano biskvitast sa malo zaostale slatkoće a u aromi preovladava lagana cvjetna hmeljnost koja dolazi od plemenitog njemačkog hmelja. 
Dlaku mijenja ali nikada ćud.',
'images\\products\\Pilsner.jpg',
@companyId, @beerId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @bottle, 5.00);
insert into productserving(productId, servingId, price)
values(@productId, @hugeBottle, 8.00);
-- Online shop
insert into shopproductserving(productId, servingId, price, photoUri, shopAmountId)
values(@productId, @bottle, 5.00,'images\\products\\Pilsner.jpg', @smallerPackage);
insert into shopproductserving(productId, servingId, price, photoUri, shopAmountId)
values(@productId, @hugeBottle, 8.00,'images\\products\\pilsner2l.png', @hugePackage);

insert into product(hexColor, name, description, photoUri, companyId, productTypeId)
values('#01c302', 'Ipa',
'Naša MCB IPA je verzija američkog India Pale Ale-a. Zanatsko/kraft pivo mutno narandžaste boje sa laganom bijelom pjenom otkriva arome slatke narandže i citrusa sa aromom tropskog voća u pozadini. 
Balansiran profil slada daje ugodno puno tijelo koje nadopunjuje umjerena gorčina u ukusu. MCB IPA je u donjoj granici sa 6% alkohola i možete je piti skoro cijelo veče sve dok ne popijete previše. 
MCB IPA, samo za iskusne znalce 🍻🍺.',
'images\\products\\Ipa.jpg',
@companyId, @beerId);
set @productId = last_insert_id();
insert into productserving(productId, servingId, price)
values(@productId, @bottle, 5.00);
insert into productserving(productId, servingId, price)
values(@productId, @hugeBottle, 8.00);
-- Online shop
insert into shopproductserving(productId, servingId, price, photoUri, shopAmountId)
values(@productId, @bottle, 5.00,'images\\products\\Ipa.jpg', @smallerPackage);
insert into shopproductserving(productId, servingId, price, photoUri, shopAmountId)
values(@productId, @hugeBottle, 8.00,'images\\products\\ipa2l.png', @hugePackage);

-- Cocktails
insert into product(hexColor, name, description, photoUri, companyId, productTypeId)
values('#e4b503', 'New Green Coctail',
'Kombinacija master pilsnera i kiwi pirea, idealan za početak ljeta 😊🍹🍺🥝🌞',
'images\\products\\NewGreenCocktail.jpg',
@companyId, @coctailId);

insert into product(hexColor, name, description, photoUri, companyId, productTypeId)
values('#e4b503', 'Cocktail Gradski',
'Kombinacija master pilsner i ananas pire, idealno društvo pod suncobranom 😊🍺🍸',
'images\\products\\GradskiCocktail.jpg',
@companyId, @coctailId);

insert into product(hexColor, name, description, photoUri, companyId, productTypeId)
values('#338ec7', 'Cocktail Venecija',
'Kombinacija craft banjalučko i jagoda pire, za duge ljetne razgovore 😊🍺🍸',
'images\\products\\VenecijaCocktail.jpg',
@companyId, @coctailId);

insert into product(hexColor, name, description, photoUri, companyId, productTypeId)
values('#ce7801', 'Cocktail Kanjon',
'Kombinacija pale ale i yuzu pire, naš odgovor na vrelinu ljeta 😊🍺🍸',
'images\\products\\KanjonCocktail.jpg',
@companyId, @coctailId);

insert into product(hexColor, name, description, photoUri, companyId, productTypeId)
values('#338ec7', 'Coctail Kastel',
'Kombinacija craft banjalučko i kajsija pire, uživajte 😊🍺🍸',
'images\\products\\DefaultCocktail.jpg',
@companyId, @coctailId);

insert into product(hexColor, name, description, photoUri, companyId, productTypeId)
values('#ce7801', 'Coctail Abacija',
'Kombinacija pale ale i passion fruit pire, neodoljivi spoj voća i piva 😊🍺🍸',
'images\\products\\AbacijaCocktail.jpg',
@companyId, @coctailId);

-- Events
insert into event(companyId, title, organizer, beginOn, endOn, durationInHours, location, photoUri, description)
values(@companyId, 'Beer Connoisseur Course Pivoved BL', 'Bar Akademija, Barista & Mezza Specialty Coffee','2019-12-18 17:30:00','2019-12-18 19:30:00',2,'The Master Craft Brewery'
,'images\\events\\beerConnoisseur.jpg',
'Zdravo svima!
Posle detaljnog izučavanja kulture uživanja piva, klasifikacije piva, degustacije nebrojenih stilova i vremena provedenog u veselom pivarenju u kraft pivarama u BiH, Srbiji i Rusiji, radi smo podijeliti nešto stečenog znanja sa vama, pivskim i kafanskim entuzijastima!
Zahvaljujući oživljavanju i popularizaciji kraft pivarstva, kultura uživanja piva i upraivanja sa sirom i klopom uopšte, raste stalno! Otvaranje nebrojenih zanatskih pivara kod nas je takođe fakat koji pridonosi sve većoj popularizaciji krafta. Situacija se sve više mijenja i percepcija piva i njegovo mjesto u pop kulturi dobija zasluženo mjesto. Situacija je natoliko ozbiljna, da se čak velike pivarske kompanije okreću kreaciji novih edicija drugih stilova piva pored standardnog svijetlog lagera, i kreiraju razne tradicionalne stilove belgijskih i njemačkih piva! Obuka se naziva Pivoved kao složenica iz imenice Pivo i staroslovenske imenice Veda(duboko znanje, vedati – vjeti – znati). Nadamo se, ne treba dalje objašnjavati 😀
Obuka, koju smo mi pripremili traje 3 dana i sastoji se od:
OSNOVE PIVARSTVA
Fakti kod varenja piva, sastojaka od kojih se pravi pivo u različitim reonima, tradicionalna percepcija i pravac razvoja pivske kulture; Definicija piva;
ISTORIJA
Kulturni i istorijski procesi koji su uticali i izazivali razne pojave kod procesa nastanka, definicije i razvoja raznih stilova piva;
STILOVI PIVA
Definicija stilova, opšta klasifikacija pivskih stilova i izbor sastojaka kod varenja, tradicionalni i netradicionalni stilovi, razlikovanje i percepcija! Tasting i poređenje 24 raznih stilova uključujući Portere i Stoute, Weissbier i Witbier, Lambic i Kriek, IPA i APA, DIPA, klasične Pilsnere te neobično svarene pivske tvari i pivske stilove. Svaki dan radimo degustaciju i poređenje 8 stila piva!
SASTOJCI
Definisanje vode, žitarice, hmelja i kvasca. Principi korištenja sastojaka u procesu varenja i uticaj izbora raznih vrsta hmelja, slada i kvasca na definisanje konačnog proizvoda!
ZAKLJUČAK
Definitivna besjeda uz veliko pivo po izboru i podjela diploma o uspješnom učešću na kursu!
PRIJAVE I DETALJI
http://www.shakersbartendingacademy.info/prijava-na-pivoved/
LOKACIJA:
MCB Pivara, Banja Luka (1 dan - Proces proizvodnje i taproom tasting)
Bar Akademija Tasting Room, Banja Luka (2 dana - tasting drugih stilova)');

insert into event(companyId, title, organizer, beginOn, endOn, durationInHours, location, photoUri, description)
values(@companyId, 'Promocija prvog gluten free piva u BiH', 'The Master Craft Brewery','2019-04-08 20:00',
'2019-04-08 00:00', 4, 'The Master Craft Brewery','images\\events\\glutenFree.jpg',
'Došlo vrijeme za degustaciju prvog bezglutenskog piva u BiH.
👉 JOKER - Pale Ale gluten free
"Serviraj za pobjedu 😍"
🕗 Ponedjeljak 08.04. u 20h
The Master Craft Brewery
Nutricionistkinja Dragana Lošić će vam reći nešto više o osobinama našeg piva.
Čast nam je da to veče ugostimo jednog od najboljih svjetskih kuvara, Chef Art Smith-a, ličnog kuvara Baraka Obame i Oprah Winfrey koji će spremati burgere specijalno za vas.
#mastercraft #pivara #craftbeer #banjaluka #beer #beerlovers #beergeek #beerlife #beerporn #beerstagram #pivo #nightlife #banjalukanightlife #instabeer
#joker #paleale #glutenfree');

insert into event(companyId, title, organizer, beginOn, endOn, durationInHours, location, photoUri, description)
values(@companyId, '2. Kraft Arena', 'The Master Craft Brewery','2019-03-27 18:00',
null, 0, 'The Master Craft Brewery','images\\events\\kraftArena.jpg',
'📣📣📣The Master Craft Brewery vas poziva na 2. Kraft arenu, srijeda 27.03.2019. 18h📣📣📣
Učesnici: Garden brewery CRO, Kabinet brewery SRB, Mammut beer factory MNE, Gvint pivara SRB, Nova Runda brewery CRO, specijalni gost Aleksandar Srdić dir. marketinga Belgrade Beer Fest i suvlasnik kultnog Gunners pub.
Najbolje organizovane pivare iz regiona i šire, vrhunska piva, dobro druženje.');

insert into event(companyId, title, organizer, beginOn, endOn, durationInHours, location, photoUri, description)
values(@companyId, '5rolej band', 'The Master Craft Brewery','2019-02-13 20:00',
null, 0, 'The Master Craft Brewery','images\\events\\5rolejBand.jpg',
'💃🕺🎵🎶🎸🎙🥁🍻🍺');

insert into event(companyId, title, organizer, beginOn, endOn, durationInHours, location, photoUri, description)
values(@companyId, '1. Kraft arena', 'The Master Craft Brewery','2019-01-23 18:00',
'2019-01-23 23:00', 5, 'The Master Craft Brewery','images\\events\\prvaKraftArena.jpg',
'Ljubitelji piva, krafteri, drugari, dodjite 23.01.2019. (srijeda) u prvu arenu zanatskog / kraft piva koja će se održati u The Master Craft Brewery.😊
Predstaviće vam se:
-DRAMA craft brewery
-GORŠTAK zanatska pivara
-THE MASTER CRAFT BREWERY brewery & tap room
-ILLYRICUM brewery & tap room
-TRIGGER craft brewery
-DOBOJSKA zanatska pivara
Za prvih 100 posjetilaca posebno iznenadjenje!👌');

insert into event(companyId, title, organizer, beginOn, endOn, durationInHours, location, photoUri, description)
values(@companyId, 'Kuvamo WHEAT, pšenično pivo', 'The Master Craft Brewery','2018-11-12 12:00',
'2018-11-13 23:00', 0, 'The Master Craft Brewery','images\\events\\production.jpg',
'Drugari, ponedeljak i utorak popodne/uveče kuvamo Wheat, pšenično pivo 🤩😍. Kao i uvjek, mi kuvamo vi uživate 🍔🍗🥩🍖🥩🍟🍻🍺🥂🍷.
Svoje njemačko porijeklo zaboravilo je već na prvim miljama Atlantika i odmah po dolasku na američko tlo usvojilo vrijednosti novog kontinenta, prirodnost i posebnu lakoću.Tako je nastalo ovo suvo, lagano i prirodno mutno pivo kremaste teksture, pšeničnog tijela i hmeljne duše.
Lagano se pije a teško zaboravlja! Pivili 🍻🍺.
#mastercraft #pivara #craftbeer #banjaluka #beer #beerlovers #beergeek #beerlife #beerporn #beerstagram #pivo #nightlife #banjalukanightlife #instabeer #wheat #psenicnopivo #pivili');

insert into event(companyId, title, organizer, beginOn, endOn, durationInHours, location, photoUri, description)
values(@companyId, 'Kuvamo Pale Ale', 'The Master Craft Brewery','2018-11-05 12:00',
'2018-11-06 22:30', 0, 'The Master Craft Brewery','images\\events\\production2.jpg',
'Pažnja, pažnja 📣📣📣 ponedeljak i utorak Bogovi hmelja ponovo silaze u Master pivaru i kuvamo PALE ALE !
Vi uživate u pivu, klopi, početku nove radne nedelje, dok mi kuvamo pale ale za neka sledeća druženja 😍🤩.
Čekamo vas, pivili 🍻🍺!');

insert into event(companyId, title, organizer, beginOn, endOn, durationInHours, location, photoUri, description)
values(@companyId, 'Kuvamo India Pale Ale', 'The Master Craft Brewery','2018-10-30 12:00',
'2018-10-31 23:00', 0, 'The Master Craft Brewery','images\\events\\production.jpg',
'Dođite da zajedno kuvamo IPA-u!
Utorak i srijeda ➡️ popodne / veče.
Izrazito popularno pivo u krugovima ljubitelja craft-a.
India Pale Ale datira još iz davnih 1700-tih kada su Britanske pivare počele dodavati dodatni hmelj u pivo koje se slalo u daleke, toplije krajeve. Ideja je bila da se doda više hmelja zbog očuvanja pića, posebno na dugim putovanjima. Pivo ne samo da je preživjelo putovanje, nego se ispostavilo da je još i neslućeno dobilo na kvalitetu. Tako je rođena prva IPA. Pivo se najviše slalo britanskim vojnicima u Indiji, koji su ga obožavali. Zbog toga je i dobilo naziv India Pale Ale.
Ako niste probali ovo pivo, zamislite ukus tipičnog Pale Ale-a, pojačajte dozu alkohola, gorčinu i ukus hmelja i dobićete IPA-u.
Vidimo se! Pivili!🍻');

insert into event(companyId, title, organizer, beginOn, endOn, durationInHours, location, photoUri, description)
values(@companyId, 'Kuvanje Russian Imperial Stout', 'The Master Craft Brewery','2018-10-26 12:00',
'2018-10-26 23:00', 11, 'The Master Craft Brewery','images\\events\\production.jpg',
'Kuvamo Russian Imperial Stout - zimski MCB specijal ➡️ petak popodne / veče
Pivo koje donosi čaroliju ruske zime uz vrhunske gurmanske delicije i dezerte.
Russian imperial stout, odnosno Ruski carski stout, stil je piva koji je nastao u 18. vijeku u Engleskoj. Kako je Stout tradicionalno englesko pivo, ali mu je sadržaj alkohola u prosjeku 4%, ruski carski dvor zatražio je od Engleza da im naprave Stout koji će imati duplo, ako ne i više alkohola u volumenu. Baš u ruskom stilu.
Riječ je o tamnom izuzetno snažnom pivu, kako mu i sam naziv kaže. Jedna je od najalkoholnijih verzija ove vrste piva, sa 8.5% alkohola, ali nevjerovatnog slatkasto-kremastog ukusa. Njegova plemenita aroma inspirisana rogačem, suvom smokvom i šljivom doprinosi tome da se ovo pivo odlično slaže kako uz mesne delicije, tako i uz deserte.
Vidimo se, pivili 🍻🍺!');

insert into event(companyId, title, organizer, beginOn, endOn, durationInHours, location, photoUri, description)
values(@companyId, 'Kuvanje Pale Ale', 'The Master Craft Brewery','2018-11-12 18:00',
'2018-11-13 18:00', 24, 'The Master Craft Brewery','images\\events\\production2.jpg',
'Pažnja, pažnja: u petak i subotu Bogovi hmelja silaze u MCB i kuvamo PALE ALE !
Vi uživate u pivu, klopi, vikendu. Mi kuvamo za Vas pale ale za neke sledeće vikende 😍🍻🍺.
Čekamo vas!');

insert into event(companyId, title, organizer, beginOn, endOn, durationInHours, location, photoUri, description)
values(@companyId, '48. Krug sa Yu grupom (izložba crteža, druženje, kraft pivo)', 'The Master Craft Brewery','2018-07-07 08:00',
'2018-07-08 12:00', 16, 'The Master Craft Brewery','images\\events\\rockAndRoll.jpg',
'48. krug rock ‘n’ rolla je izložba crteža posvećena Yu grupi i njihovom 48. godina dugom radu.
Izložba se otvara u subotu 30. juna u 18h u The Master Craft Brewery pivari u ulici Bulevar Vojvode Stepe Stepanovića 44 A, pored Ugostiteljske škole. Gdje ćemo se sa Jelićima, ali i svim ljubiteljima piva i umjetnosti družiti uz vrhunska kraft piva.
Gosti izložbe su članovi benda Yu grupa (Žika Jelić, Dragi Jelić, Petar Jelić)
Izložbu čini 48 crteža ,a svaki od njih nosi naziv jedne od pjesama Yu grupe, one kojom je inspirisan, a samo otvaranje upotpuniće mini vremeplov o Jelićima, te muzički dio programa.
Ideja je krenula još prije dvije-tri godine kada je Marija Đurić bila na jednom od njihovih koncerata.
"Čekali smo neki stari bend a kiša je lila, no onda je na binu izašao Žika Jelić sa svojom Yu grupom i
 zaustavio kišu… Nakon toga rodila se ogromna, neizmjerna ljubav prema onome što oni rade 
 i stvaraju evo već skoro pola vijeka, pritom ostaju dosljedni sebi, originalni", kaže Marija.');

-- Quotes
 insert into quote(QuoteText, Author, CompanyId, CreatedOn)
	values('24 sata u danu, 24 piva u gajbi. Koincidencija? Mislim da nije.'
	,'Steven Wright', @companyId, utc_timestamp());
insert into quote(QuoteText, Author, CompanyId, CreatedOn)
	values('DA LI STE ZNALI: Energetska vrijednost jedne litre piva odgovara energetskoj vrijednosti jedne litre punomasnog mlijeka.'
	,'', @companyId, utc_timestamp());
insert into quote(QuoteText, Author, CompanyId, CreatedOn)
	values('Pored muzike najbolja stvar na svijedu je PIVO.'
	,'Carson McCullers', @companyId, utc_timestamp());
insert into quote(QuoteText, Author, CompanyId, CreatedOn)
	values('Pivo dolazi na svijet zahvaljujući ljudskom znoju i božjoj ljubavi.'
	,'Sveti Arnold, zaštitinik pivara', @companyId, utc_timestamp());
insert into quote(QuoteText, Author, CompanyId, CreatedOn)
	values('Svako može piti pivo, ali potrebna je inteligencija da se u njemu uživa.'
	,'Stephen Beaumont', @companyId, utc_timestamp());
insert into quote(QuoteText, Author, CompanyId, CreatedOn)
	values('Dajte mi ženu koja voli pivo i pokoriću svijet.'
	,'Kajzer Vilhelm II', @companyId, utc_timestamp());
insert into quote(QuoteText, Author, CompanyId, CreatedOn)
	values('Pivo je dokaz da nas Bog voli i da želi da budemo srećni.'
	,'Benjamin Franklin', @companyId, utc_timestamp());
insert into quote(QuoteText, Author, CompanyId, CreatedOn)
	values('Ako u riječi STRAST zamijenite četiri slova, a dva izbacije, dobićete riječ PIVO.'
	,'N.N.', @companyId, utc_timestamp());
insert into quote(QuoteText, Author, CompanyId, CreatedOn)
	values('Alkohol je naš najveći neprijatelj, a samo kukavice bježe od neprijatelja.'
	,'', @companyId, utc_timestamp());
    
-- Gallery
insert into `mastercraftbrewery`.`gallery`
(`Name`, `Description`, `CompanyId`, `CreatedOn`)
values ('Prijatan ambijent','Posjetite nas!', @companyId, utc_timestamp());
set @galleryId = last_insert_id();

insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Ambijent1.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Ambijent2.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Ambijent3.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Ambijent4.jpg', @galleryId);

insert into `mastercraftbrewery`.`gallery`
(`Name`, `Description`, `CompanyId`, `CreatedOn`)
values ('Dr Nele Karajlić','Promocija knjiga Fajront u Sarajevu i Solunska 28 i nezaboravno druženje', @companyId, utc_timestamp());
set @galleryId = last_insert_id();

insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Fajront1.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Fajront2.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Fajront3.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Fajront4.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Fajront5.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Fajront6.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Fajront7.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Fajront8.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Fajront9.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Fajront10.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Fajront11.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Fajront12.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Fajront13.jpg', @galleryId);

insert into `mastercraftbrewery`.`gallery`
(`Name`, `Description`, `CompanyId`, `CreatedOn`)
values ('Festival zanatskog piva','Osvojeno prvo mjesto!', @companyId, utc_timestamp());
set @galleryId = last_insert_id();

insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Festival0.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Festival00.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Festival01.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Festival1.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Festival2.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Festival3.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Festival4.jpg', @galleryId);

insert into `mastercraftbrewery`.`gallery`
(`Name`, `Description`, `CompanyId`, `CreatedOn`)
values ('KAKO MI TO RADIMO...','Pogledajte proces proizvodnje u nekoliko koraka :-)', @companyId, utc_timestamp());
set @galleryId = last_insert_id();

insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Kmtr0.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Kmtr1.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Kmtr2.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Kmtr3.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Kmtr4.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Kmtr5.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Kmtr6.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Kmtr7.jpg', @galleryId);
insert into `mastercraftbrewery`.`mediafile`
(`Uri`, `GalleryId`)
values ('images\\gallery\\Kmtr8.jpg', @galleryId);

