----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    08/01/2015 
-- Design Name: 
-- Module Name:    test9_13 - Behavioral 
-- Project Name: 
-- Target Devices: 
-- Tool versions: 
-- Description: 
--
-- Dependencies: 
--
-- Revision: 
-- Revision 0.01 - File Created
-- Additional Comments: 
--
----------------------------------------------------------------------------------
library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use ieee.std_logic_unsigned.all;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity uc_top is
    Port ( clk : in std_logic;
			  btn_reset: in std_logic;
			  sw : in std_logic_vector (7 downto 0);
			  btn : in std_logic_vector (3 downto 0);
			  anod : out std_logic_vector (3 downto 0);
			  segment : out std_logic_vector(7 downto 0);
           led : out  STD_LOGIC_VECTOR (7 downto 0);
			  port_intrare1 : in std_logic_vector (7 downto 0); -- conectat la JA
			  port_iesire1 : out std_logic_vector (7 downto 0); -- conectat la JB
			  port_in_out1 : inout std_logic_vector (7 downto 0); -- conectat la JC
			  tx : out std_logic;
			  rx : in std_logic);
end uc_top;

architecture Behavioral of uc_top is

constant nop :		std_logic_vector(7 downto 0) := "00000000"; -- 0
constant movla :	std_logic_vector(7 downto 0) := "00000001"; -- 1
constant addla :	std_logic_vector(7 downto 0) := "00000010"; -- 2
constant jmp :		std_logic_vector(7 downto 0) := "00000011"; -- 3
constant movaf :	std_logic_vector(7 downto 0) := "00000100"; -- 4
constant movfa :	std_logic_vector(7 downto 0) := "00000101"; -- 5
constant jpz :		std_logic_vector(7 downto 0) := "00000110"; -- 6
constant jpc :		std_logic_vector(7 downto 0) := "00000111"; -- 7
constant jpnz :	std_logic_vector(7 downto 0) := "00001000"; -- 8
constant jpnc :	std_logic_vector(7 downto 0) := "00001001"; -- 9
constant subla :	std_logic_vector(7 downto 0) := "00001010"; -- 10
constant addfa :	std_logic_vector(7 downto 0) := "00001011"; -- 11
constant subfa :	std_logic_vector(7 downto 0) := "00001100"; -- 12
constant incf :	std_logic_vector(7 downto 0) := "00001101"; -- 13
constant decf :	std_logic_vector(7 downto 0) := "00001110"; -- 14
constant call :	std_logic_vector(7 downto 0) := "00001111"; -- 15
constant ret :		std_logic_vector(7 downto 0) := "00010000"; -- 16
constant callz :	std_logic_vector(7 downto 0) := "00010001"; -- 17
constant callnz:	std_logic_vector(7 downto 0) := "00010010"; -- 18
constant callc	:	std_logic_vector(7 downto 0) := "00010011"; -- 19
constant callnc :	std_logic_vector(7 downto 0) := "00010100"; -- 20
constant andla :	std_logic_vector(7 downto 0) := "00010101"; -- 21
constant orla :	std_logic_vector(7 downto 0) := "00010110"; -- 22
constant andfa :	std_logic_vector(7 downto 0) := "00010111"; -- 23
constant orfa :	std_logic_vector(7 downto 0) := "00011000"; -- 24
constant xorla :	std_logic_vector(7 downto 0) := "00011001"; -- 25
constant xorfa :	std_logic_vector(7 downto 0) := "00011010"; -- 26
constant nega :	std_logic_vector(7 downto 0) := "00011011"; -- 27
constant rlca :	std_logic_vector(7 downto 0) := "00011100"; -- 28
constant rrca :	std_logic_vector(7 downto 0) := "00011101"; -- 29
constant outa :	std_logic_vector(7 downto 0) := "00011110"; -- 30
constant ina :		std_logic_vector(7 downto 0) := "00011111"; -- 31
constant page :	std_logic_vector(7 downto 0) := "00100000"; -- 32
constant movcaf :	std_logic_vector(7 downto 0) := "00100001"; -- 33
constant movcfa :	std_logic_vector(7 downto 0) := "00100010"; -- 34
-- ---------------
constant achitare_intrerupere_0 :	std_logic_vector(7 downto 0) := "00100011"; -- 35
constant achitare_intrerupere_1 :	std_logic_vector(7 downto 0) := "00100100"; -- 36
constant achitare_intrerupere_2 :	std_logic_vector(7 downto 0) := "00100101"; -- 37
constant achitare_intrerupere_3 :	std_logic_vector(7 downto 0) := "00100110"; -- 38
constant achitare_intrerupere_4 :	std_logic_vector(7 downto 0) := "00100111"; -- 39
constant achitare_intrerupere_5 :	std_logic_vector(7 downto 0) := "00101000"; -- 40
constant achitare_intrerupere_6 :	std_logic_vector(7 downto 0) := "00101001"; -- 41
-- ---------------
constant reti_0 : std_logic_vector(7 downto 0) := "00101010"; -- 42
constant reti_1 : std_logic_vector(7 downto 0) := "00101011"; -- 43
constant reti_2 : std_logic_vector(7 downto 0) := "00101100"; -- 44
constant reti_3 : std_logic_vector(7 downto 0) := "00101101"; -- 45
constant reti_4 : std_logic_vector(7 downto 0) := "00101110"; -- 46
constant reti_5 : std_logic_vector(7 downto 0) := "00101111"; -- 47
constant reti_6 : std_logic_vector(7 downto 0) := "00110000"; -- 48
-- ---------------
constant savecz : std_logic_vector(7 downto 0) := "00110001"; -- 49
constant restcz : std_logic_vector(7 downto 0) := "00110010"; -- 50

	signal q : std_logic_vector(0 downto 0) := (others => '0');
	signal max_tick_q : std_logic := '0';
	
-- ---------------

component modul_7seg
	port (
				clk : in  STD_LOGIC;
				port_in : in  STD_LOGIC_VECTOR (3 downto 0);
				nr_port : in  STD_LOGIC_VECTOR (1 downto 0);
				anod : out  STD_LOGIC_VECTOR (3 downto 0);
				segment : out  STD_LOGIC_VECTOR (7 downto 0)
			);
end component;

COMPONENT mem1
  PORT (
    clka : IN STD_LOGIC;
    wea : IN STD_LOGIC_VECTOR(0 DOWNTO 0);
    addra : IN STD_LOGIC_VECTOR(12 DOWNTO 0);
    dina : IN STD_LOGIC_VECTOR(7 DOWNTO 0);
    douta : OUT STD_LOGIC_VECTOR(7 DOWNTO 0)
  );
END COMPONENT;

COMPONENT timer
	generic(MSB_prescaler : integer);
	PORT(
		clk : IN std_logic;
		paralel : IN std_logic_vector(7 downto 0);          
		contor : OUT std_logic_vector(7 downto 0);
		tick : OUT std_logic
	);
END COMPONENT;

	COMPONENT uart
	PORT(
		clk : IN std_logic;
		reset : IN std_logic;
		rd_uart : IN std_logic;
		wr_uart : IN std_logic;
		rx : IN std_logic;
		w_data : IN std_logic_vector(7 downto 0);          
		tx_full : OUT std_logic;
		rx_empty : OUT std_logic;
		r_data : OUT std_logic_vector(7 downto 0);
		tx : OUT std_logic
		);
	END COMPONENT;
	
   --numele starilor
   type state_type is (extrag_op, citesc_mem, scriu_mem, execut_op, execut_op1); 
   signal state, next_state : state_type; 
   -- semnalele interne pentru state-machine
   -- alte semnale
	--signal reset_intern : std_logic;
	--signal next_reset_intern : std_logic;
	signal reset : std_logic;
	
	-- ----------------------------
    signal wea : STD_LOGIC_VECTOR(0 DOWNTO 0);
    signal next_wea : STD_LOGIC_VECTOR(0 DOWNTO 0);	 
    signal adresa_ram : STD_LOGIC_VECTOR(7 DOWNTO 0);
    signal next_adresa_ram : STD_LOGIC_VECTOR(7 DOWNTO 0);
    signal din_ram : STD_LOGIC_VECTOR(7 DOWNTO 0);
	 signal next_din_ram : STD_LOGIC_VECTOR(7 DOWNTO 0);
    signal dout_ram : STD_LOGIC_VECTOR(7 DOWNTO 0);
	-- ----------------------------
	signal acumulator : std_logic_vector(8 downto 0) := (others => '0');
	signal next_acumulator : std_logic_vector(8 downto 0) := (others => '0');
	signal contor_program : std_logic_vector(7 downto 0) := (others => '0');
	signal next_contor_program : std_logic_vector(7 downto 0) := (others => '0');
	signal instructiune : std_logic_vector(7 downto 0) := (others => '0');
	signal next_instructiune : std_logic_vector(7 downto 0) := (others => '0');
	signal temp_contor_program : std_logic_vector(7 downto 0) := (others => '0');
	signal next_temp_contor_program : std_logic_vector(7 downto 0) := (others => '0');
	signal temp_acumulator : std_logic_vector(8 downto 0) := (others => '0');
	signal next_temp_acumulator : std_logic_vector(8 downto 0) := (others => '0');
	signal zf : std_logic := '0';
	signal next_zf : std_logic := '0';
	signal contor_stiva : std_logic_vector(7 downto 0) := (others => '1');
	signal next_contor_stiva : std_logic_vector(7 downto 0) := (others => '1');
	signal port_iesire : std_logic_vector(7 downto 0) := (others => '0');
	signal next_port_iesire : std_logic_vector(7 downto 0) := (others => '0');
	signal numar_port_iesire : std_logic_vector(7 downto 0) := (others => '0');
	signal next_numar_port_iesire : std_logic_vector(7 downto 0) := (others => '0');
	signal port_intrare : std_logic_vector(7 downto 0) := (others => '0');
	signal next_port_intrare : std_logic_vector(7 downto 0) := (others => '0');
	signal numar_port_intrare : std_logic_vector(7 downto 0) := (others => '0');
	signal next_numar_port_intrare : std_logic_vector(7 downto 0) := (others => '0');
	signal led_i : std_logic_vector(7 downto 0);
	signal next_led_i : std_logic_vector(7 downto 0);
	signal pagina : std_logic_vector(4 downto 0) := (others => '0'); -- 8k !!!!!!
	signal next_pagina : std_logic_vector(4 downto 0) := (others => '0');
	signal adresa_extinsa : std_logic_vector (12 downto 0);
	signal temp_pagina : std_logic_vector(4 downto 0);
	signal next_temp_pagina : std_logic_vector(4 downto 0);
	signal pagina_selectata_transfer : std_logic_vector(4 downto 0);
	signal next_pagina_selectata_transfer : std_logic_vector(4 downto 0);

-- ----------------
signal port_in_intern 	: std_logic_vector(3 downto 0);
signal nr_port_intern 	: std_logic_vector(1 downto 0);
signal next_port_in_intern 	: std_logic_vector(3 downto 0);
signal next_nr_port_intern 	: std_logic_vector(1 downto 0);
-- ---------------
-- intreruperi

signal int0 : std_logic := '0';
signal int1 : std_logic := '0';
signal int2 : std_logic := '0';
signal int3 : std_logic := '0';
signal int4 : std_logic := '0';
signal int5 : std_logic := '0';
signal int6 : std_logic := '0';

signal validare_intrerupere : std_logic_vector(7 downto 0) := (others => '0');
signal next_validare_intrerupere : std_logic_vector(7 downto 0) := (others => '0');

--signal incheiere_tratare_intrerupere : std_logic_vector(6 downto 0) := (others => '0');
--signal next_incheiere_tratare_intrerupere : std_logic_vector(6 downto 0) := (others => '0');
signal incheiere_tratare_intrerupere : std_logic_vector(1 downto 0) := (others => '0');
signal next_incheiere_tratare_intrerupere : std_logic_vector(1 downto 0) := (others => '0');

-- ====== se decomenteaza cind se adauga intreruperi =========
--signal tratare_intrerupere : std_logic_vector(6 downto 0) := (others => '0');
signal tratare_intrerupere : std_logic_vector(1 downto 0) := (others => '0');

--signal stergere_intrerupere : std_logic_vector(6 downto 0) := (others => '0');
--signal aparitie_intrerupere : std_logic_vector(6 downto 0) := (others => '0');

-- -----------------------
-- timer

--signal prescaller: STD_LOGIC_VECTOR(24 DOWNTO 0);
signal constanta_timer: std_logic_vector(7 downto 0) := (others => '0');
signal next_constanta_timer: std_logic_vector(7 downto 0) := (others => '0');
signal contor_timer: std_logic_vector(7 downto 0);
signal tick_timer: std_logic;
-- ----------------
-- serial
	signal rd_uart : std_logic := '0'; -- citesc din UART octet sosit (puls unu)
	signal next_rd_uart : std_logic := '0'; -- citesc din UART octet sosit (puls unu)
	signal wr_uart : std_logic := '0'; -- scriu in UART otet de emis (puls unu)
	signal next_wr_uart : std_logic := '0'; -- scriu in UART otet de emis (puls unu)
	signal w_data : std_logic_vector(7 downto 0); -- octet de emis
	signal next_w_data : std_logic_vector(7 downto 0); -- octet de emis
	signal tx_full : std_logic := '0'; -- buffer emisie plin
	signal rx_empty : std_logic := '0'; -- buffer receptie gol
	signal r_data : std_logic_vector(7 downto 0); -- octetul receptionat
--------------------
-- numele alternative folosite

	alias cf: std_logic is acumulator(8);

-- ----------------
		
begin

afisaj: modul_7seg
	port map (
				clk 		=> clk,
				port_in 	=> port_in_intern,
				nr_port  => nr_port_intern,
				anod 		=> anod,
				segment 	=> segment
	);

ram_mem : mem1
  PORT MAP (
    clka => clk,
    wea => wea,
--    addra => adresa_ram,
	 addra => adresa_extinsa,
    dina => din_ram,
    douta => dout_ram
  );
  
timer1: timer
	generic map(MSB_prescaler => 18)
	-- clk = 100 MHz / 2**19 = aproximativ 190,7 Hz
	PORT MAP(
	clk => clk,
	paralel => constanta_timer,
	contor => contor_timer,
	tick => tick_timer
);
  
	serial: uart PORT MAP(
		clk => max_tick_q,
		reset => reset,
		rd_uart => rd_uart,
		wr_uart => wr_uart,
		rx => rx,
		w_data => w_data,
		tx_full => tx_full,
		rx_empty => rx_empty,
		r_data => r_data,
		tx => tx
	);

-- ceasul microcontrolerului
	ceas_micro: process (clk)
	begin
		if (clk'event and clk = '1') then
			q <= q + 1;
		end if;
	end process;
max_tick_q <=  q(0);

intreruperi: process (clk, int0, int1, int2, int3, int4, int5, int6,
incheiere_tratare_intrerupere)
begin

--	if (incheiere_tratare_intrerupere(0) = '1') then
--		tratare_intrerupere(0) <= '0';
--	elsif (int0'event and int0 = '1') then
--		if (validare_intrerupere(0) = '1') then
--			tratare_intrerupere(0) <= '1';
--		end if;
--	end if;

	if (incheiere_tratare_intrerupere(0) = '1') then
		tratare_intrerupere(0) <= '0';
	elsif (clk'event and clk = '1') then
		if int0 = '1' then
			if (validare_intrerupere(0) = '1') then
				tratare_intrerupere(0) <= '1';
			end if;
		end if;
	end if;

	if (incheiere_tratare_intrerupere(1) = '1') then
		tratare_intrerupere(1) <= '0';
	elsif (clk'event and clk = '1') then
		if int1 = '1' then
			if (validare_intrerupere(1) = '1') then
				tratare_intrerupere(1) <= '1';
			end if;
		end if;
	end if;

-- ============ se decomenteaza cind introduc intreruperi ===================
--	if (incheiere_tratare_intrerupere(2) = '1') then
--		tratare_intrerupere(2) <= '0';
--	elsif (int2'event and int2 = '1') then
--		if (validare_intrerupere(2) = '1') then
--			tratare_intrerupere(2) <= '1';
--		end if;
--	end if;
--
--	if (incheiere_tratare_intrerupere(3) = '1') then
--		tratare_intrerupere(3) <= '0';
--	elsif (int3'event and int3 = '1') then
--		if (validare_intrerupere(3) = '1') then
--			tratare_intrerupere(3) <= '1';
--		end if;
--	end if;
--
--	if (incheiere_tratare_intrerupere(4) = '1') then
--		tratare_intrerupere(4) <= '0';
--	elsif (int4'event and int4 = '1') then
--		if (validare_intrerupere(4) = '1') then
--			tratare_intrerupere(4) <= '1';
--		end if;
--	end if;
--
--	if (incheiere_tratare_intrerupere(5) = '1') then
--		tratare_intrerupere(5) <= '0';
--	elsif (int5'event and int5 = '1') then
--		if (validare_intrerupere(5) = '1') then
--			tratare_intrerupere(5) <= '1';
--		end if;
--	end if;
--
--	if (incheiere_tratare_intrerupere(6) = '1') then
--		tratare_intrerupere(6) <= '0';
--	elsif (int6'event and int6 = '1') then
--		if (validare_intrerupere(6) = '1') then
--			tratare_intrerupere(6) <= '1';
--		end if;
--	end if;
-- ============ se decomenteaza cind introduc intreruperi ===================

end process;


   SYNC_PROC: process (clk, reset)
   begin
		if (reset = '1') then
-- strict necesar a fi initializate la RESET
			state <= extrag_op;
			contor_program <= (others => '0');
			contor_stiva  <= (others => '1');
			pagina  <= (others => '0');
			validare_intrerupere  <= (others => '0');
			zf <= '0';

-- astea nu cred ca e necesar sa le initializez
			acumulator  <= (others => '0');
			instructiune  <= (others => '0');
			port_iesire  <= (others => '0');
			numar_port_iesire  <= (others => '0');
			port_intrare  <= (others => '0');
			numar_port_intrare  <= (others => '0');
			pagina_selectata_transfer  <= (others => '0');
			incheiere_tratare_intrerupere  <= (others => '0');
			constanta_timer  <= (others => '0');
			
			--reset_intern <= '0';
		elsif (clk'event and clk = '1') then
			if max_tick_q = '1' then
            state <= next_state;
				contor_program <= next_contor_program;
				acumulator <= next_acumulator;
				instructiune <= next_instructiune;
				temp_contor_program <= next_temp_contor_program;
				temp_acumulator <= next_temp_acumulator;
				contor_stiva <= next_contor_stiva;
				port_iesire <= next_port_iesire;
				numar_port_iesire <= next_numar_port_iesire;
				port_intrare <= next_port_intrare;
				numar_port_intrare <= next_numar_port_intrare;
				zf <= next_zf;
				next_din_ram <= din_ram;
				next_wea <= wea;
				next_adresa_ram <= adresa_ram;
				pagina <= next_pagina;
				temp_pagina <= next_temp_pagina;
				pagina_selectata_transfer <= next_pagina_selectata_transfer;
				-- ---------
				-- afisaj LED si 7 segmente
				led_i <= next_led_i;
				nr_port_intern <= next_nr_port_intern;
				port_in_intern <= next_port_in_intern;
				-- ---------------------------------------
				-- intreruperi
				incheiere_tratare_intrerupere <= next_incheiere_tratare_intrerupere;
				validare_intrerupere <= next_validare_intrerupere;
				-- ---------------------------------
				-- timer
				constanta_timer <= next_constanta_timer;
				-- ---------------------------------
				-- uart
				rd_uart <= next_rd_uart;
				wr_uart <= next_wr_uart;
				w_data <= next_w_data;
				
				--reset_intern <= next_reset_intern;
				
         -- assign other outputs to internal signals
        else
            state <= state;
				contor_program <= contor_program;
				acumulator <= acumulator;
				instructiune <= instructiune;
				temp_contor_program <= temp_contor_program;
				temp_acumulator <= temp_acumulator;
				contor_stiva <= contor_stiva;
				port_iesire <= port_iesire;
				numar_port_iesire <= numar_port_iesire;
				port_intrare <= port_intrare;
				numar_port_intrare <= numar_port_intrare;
				zf <= zf;
				next_din_ram <= next_din_ram;
				next_wea <= next_wea;
				next_adresa_ram <= next_adresa_ram;
				pagina <= pagina;
				temp_pagina <= temp_pagina;
				pagina_selectata_transfer <= pagina_selectata_transfer;
				-- ---------
				-- afisaj LED si 7 segmente
				led_i <= led_i;
				nr_port_intern <= nr_port_intern;
				port_in_intern <= port_in_intern;
				-- ---------------------------------------
				-- intreruperi
				incheiere_tratare_intrerupere <= incheiere_tratare_intrerupere;
				validare_intrerupere <= validare_intrerupere;
				-- ---------------------------------
				-- timer
				constanta_timer <= constanta_timer;
				-- ---------------------------------
				-- uart
				rd_uart <= rd_uart;
				wr_uart <= wr_uart;
				w_data <= w_data;
				
				--reset_intern <= reset_intern;
				
         -- assign other outputs to internal signals
         end if;        
      end if;
   end process;
  
-- DEFINIREA IESIRILOR
-- ===================
     --MOORE State-Machine - Outputs based on state only
   OUTPUT_DECODE: process (state, port_iesire, next_numar_port_iesire,
	numar_port_iesire, led_i, nr_port_intern, port_in_intern,
	constanta_timer)
   begin
		next_led_i <= led_i; --pentru a evita latches
		next_nr_port_intern <= nr_port_intern; --pentru a evita latches
		next_port_in_intern <= port_in_intern; --pentru a evita latches
		next_constanta_timer <= constanta_timer; --pentru a evita latches
      --insert statements to decode internal output signals
      --below is simple example

		if (state = extrag_op) then
			if (numar_port_iesire = "00000000") then --port 0 - iesire pe leg
				next_led_i <= port_iesire;
			end if;
			if (numar_port_iesire = "00000001") then --port 1 - iesire afisaj 7 segmente - minute
						next_nr_port_intern <= "00"; -- portul afisajului 7 segmente este zero pentru minute
						next_port_in_intern <= port_iesire(3 downto 0);
			end if;
			if (numar_port_iesire = "00000010") then --port 2 - iesire afisaj 7 segmente - zeci minute
						next_nr_port_intern <= "01";
						next_port_in_intern <= port_iesire(3 downto 0);
			end if;
			if (numar_port_iesire = "00000011") then --port 3 - iesire afisaj 7 segmente - ore
						next_nr_port_intern <= "10";
						next_port_in_intern <= port_iesire(3 downto 0);
			end if;
			if (numar_port_iesire = "00000100") then --port 4 iesire afisaj 7 segmente - zeci ore
						next_nr_port_intern <= "11";
						next_port_in_intern <= port_iesire(3 downto 0);
			end if;
			if (numar_port_iesire = "00000101") then --port 5 - constanta de timp a circuitului timer
						next_constanta_timer <= port_iesire;
			end if;
			--port 6 rezervat pentru uart
			--port 7 rezervat pentru page
			if (numar_port_iesire = "00001000") then --port 8 - port iesire conectat la JB
						port_iesire1 <= port_iesire;
			end if;
			if (numar_port_iesire = "00001001") then --port 9 - port intrare-iesire conectat la JC
						port_in_out1 <= port_iesire;
			end if;
		end if;
				
	end process; 
-- ================================================================
  
   NEXT_STATE_DECODE: process (state,contor_program,instructiune,
	acumulator,dout_ram,zf,temp_contor_program,cf,contor_stiva,
	temp_acumulator, port_iesire, numar_port_iesire, port_intrare,
	numar_port_intrare, next_din_ram, next_wea, next_adresa_ram, next_pagina,
	pagina, temp_pagina, incheiere_tratare_intrerupere, validare_intrerupere,
	tratare_intrerupere, rx_empty, r_data, tx_full, pagina_selectata_transfer,
	next_pagina_selectata_transfer, rd_uart, wr_uart, w_data)
   begin
      --declare default state for next_state to avoid latches
      next_state <= state;  --default is to stay in current state
      --insert statements to decode next_state
      --below is a simple example
				next_acumulator <= acumulator;
				next_temp_contor_program <= temp_contor_program;
				next_contor_stiva <= contor_stiva;
				next_instructiune <= instructiune;
				next_zf <= zf;
				next_contor_program <= contor_program;
				next_port_iesire <= port_iesire;
				next_numar_port_iesire <= numar_port_iesire;
				next_numar_port_intrare <= numar_port_intrare;
				next_pagina <= pagina;
				next_temp_pagina <= temp_pagina;
				next_pagina_selectata_transfer <= pagina_selectata_transfer;
				-- ------------
				next_incheiere_tratare_intrerupere <= incheiere_tratare_intrerupere;
				next_validare_intrerupere <= validare_intrerupere;
				-- ------------
				next_rd_uart <= rd_uart;
				next_wr_uart <= wr_uart;
				next_w_data <= w_data;
				-- ------------
				din_ram <= next_din_ram;
				wea <= next_wea;
				adresa_ram <= next_adresa_ram;
				next_temp_acumulator <= temp_acumulator;
				 
		case (state) is

         when extrag_op =>
			
				if (tratare_intrerupere = "0000000") then -- nu exista cerere de intrerupere				
					wea <= "0";
					adresa_ram <= contor_program;
					next_instructiune <= dout_ram;
					next_contor_program <= contor_program + 1;
					case (dout_ram) is
						when nop =>
							next_state <= extrag_op;
						when movla =>
							next_state <= citesc_mem;
						when addla =>
							next_acumulator(8) <= '0';
							next_state <= citesc_mem;
						when subla =>
							next_acumulator(8) <= '0';
							next_state <= citesc_mem;
						when jmp =>
							next_state <= citesc_mem;
						when movaf =>
							next_state <= citesc_mem;
						when savecz =>
							next_state <= citesc_mem;
						when movfa =>
							next_state <= citesc_mem;
						when restcz =>
							next_state <= citesc_mem;
						when jpz =>
							if (zf = '1') then
								next_instructiune <= jmp;
								next_state <= citesc_mem;
							else
								next_contor_program <= contor_program + 2;
								next_state <= extrag_op;
							end if;
						when jpc =>
							if (cf = '1') then
								next_instructiune <= jmp;
								next_state <= citesc_mem;
							else
								next_contor_program <= contor_program + 2;
								next_state <= extrag_op;
							end if;
						when jpnz =>
							if (zf = '0') then
								next_instructiune <= jmp;
								next_state <= citesc_mem;
							else
								next_contor_program <= contor_program + 2;
								next_state <= extrag_op;
							end if;
						when jpnc =>
							if (cf = '0') then
								next_instructiune <= jmp;
								next_state <= citesc_mem;
							else
								next_contor_program <= contor_program + 2;
								next_state <= extrag_op;
							end if;
						when addfa =>
							next_acumulator(8) <= '0';
							next_state <= citesc_mem;
						when subfa =>
							next_acumulator(8) <= '0';
							next_state <= citesc_mem;
						when incf =>
							next_temp_acumulator <= acumulator;
							next_acumulator(8) <= '0';
							next_state <= citesc_mem;						
						when decf =>
							next_temp_acumulator <= acumulator;
							next_acumulator(8) <= '0';
							next_state <= citesc_mem;						
						when call =>
							next_state <= citesc_mem;
						when ret =>
							next_contor_program <= contor_stiva + 1;
							next_contor_stiva <= contor_stiva + 1;
							next_state <= citesc_mem;					
						when callz =>
							if (zf = '1') then
								next_instructiune <= call;
								next_state <= citesc_mem;
							else
								next_contor_program <= contor_program + 2;
								next_state <= extrag_op;
							end if;
						when callnz =>
							if (zf = '0') then
								next_instructiune <= call;
								next_state <= citesc_mem;
							else
								next_contor_program <= contor_program + 2;
								next_state <= extrag_op;
							end if;
						when callc =>
							if (cf = '1') then
								next_instructiune <= call;
								next_state <= citesc_mem;
							else
								next_contor_program <= contor_program + 2;
								next_state <= extrag_op;
							end if;
						when callnc =>
							if (cf = '0') then
								next_instructiune <= call;
								next_state <= citesc_mem;
							else
								next_contor_program <= contor_program + 2;
								next_state <= extrag_op;
							end if;
						when outa =>
							next_state <= citesc_mem;
						when ina =>
							next_state <= citesc_mem;
						when andla =>
							next_acumulator(8) <= '0';
							next_state <= citesc_mem;											
						when orla =>
							next_acumulator(8) <= '0';
							next_state <= citesc_mem;
						when andfa =>
							next_acumulator(8) <= '0';
							next_state <= citesc_mem;					
						when orfa =>
							next_acumulator(8) <= '0';
							next_state <= citesc_mem;					
						when xorla =>
							next_acumulator(8) <= '0';
							next_state <= citesc_mem;
						when xorfa =>
							next_acumulator(8) <= '0';
							next_state <= citesc_mem;					
						when nega =>
							next_acumulator(7 downto 0) <= not acumulator(7 downto 0);
							next_acumulator(8) <= '0';
							next_state <= execut_op;
						when rlca =>
							next_acumulator <= acumulator(7 downto 0) & acumulator(8);
							next_state <= execut_op;
						when rrca =>
							next_acumulator <= acumulator(0) & acumulator(8 downto 1);
							next_state <= execut_op;
						when page =>
							next_state <= citesc_mem;
						when movcaf =>
							next_state <= citesc_mem;
						when movcfa =>
							next_state <= citesc_mem;
						when reti_0 =>
							next_contor_program <= contor_stiva + 1;
							next_contor_stiva <= contor_stiva + 1;
							next_validare_intrerupere(0) <= '1';
							next_instructiune <= ret;
							next_state <= citesc_mem;					
						when reti_1 =>
							next_contor_program <= contor_stiva + 1;
							next_contor_stiva <= contor_stiva + 1;
							next_validare_intrerupere(1) <= '1';
							next_instructiune <= ret;
							next_state <= citesc_mem;					
						when reti_2 =>
							next_contor_program <= contor_stiva + 1;
							next_contor_stiva <= contor_stiva + 1;
							next_validare_intrerupere(2) <= '1';
							next_instructiune <= ret;
							next_state <= citesc_mem;					
						when reti_3 =>
							next_contor_program <= contor_stiva + 1;
							next_contor_stiva <= contor_stiva + 1;
							next_validare_intrerupere(3) <= '1';
							next_instructiune <= ret;
							next_state <= citesc_mem;					
						when reti_4 =>
							next_contor_program <= contor_stiva + 1;
							next_contor_stiva <= contor_stiva + 1;
							next_validare_intrerupere(4) <= '1';
							next_instructiune <= ret;
							next_state <= citesc_mem;					
						when reti_5 =>
							next_contor_program <= contor_stiva + 1;
							next_contor_stiva <= contor_stiva + 1;
							next_validare_intrerupere(5) <= '1';
							next_instructiune <= ret;
							next_state <= citesc_mem;					
						when reti_6 =>
							next_contor_program <= contor_stiva + 1;
							next_contor_stiva <= contor_stiva + 1;
							next_validare_intrerupere(6) <= '1';
							next_instructiune <= ret;
							next_state <= citesc_mem;					
						when others =>
							next_state <= extrag_op;
					end case;
				else -- exista cerere de intrerupere
				-- ==================================
					if (tratare_intrerupere(0) = '1') then
						next_incheiere_tratare_intrerupere(0) <= '1'; -- anunt ca sunt in tratare intrerupere si sterg tratare_intrerupere
						next_validare_intrerupere(0) <= '0'; -- dezactivez intreruperea
						next_instructiune <= achitare_intrerupere_0;
					elsif (tratare_intrerupere(1) = '1') then
						next_incheiere_tratare_intrerupere(1) <= '1';
						next_validare_intrerupere(1) <= '0';
						next_instructiune <= achitare_intrerupere_1;
-- ============ se decomenteaza cind introduc intreruperi ===================
--					elsif (tratare_intrerupere(2) = '1') then
--						next_incheiere_tratare_intrerupere(2) <= '1';
--						next_validare_intrerupere(2) <= '0';
--						next_instructiune <= achitare_intrerupere_2;
--					elsif (tratare_intrerupere(3) = '1') then
--						next_incheiere_tratare_intrerupere(3) <= '1';
--						next_validare_intrerupere(3) <= '0';
--						next_instructiune <= achitare_intrerupere_3;
--					elsif (tratare_intrerupere(4) = '1') then
--						next_incheiere_tratare_intrerupere(4) <= '1';
--						next_validare_intrerupere(4) <= '0';
--						next_instructiune <= achitare_intrerupere_4;
--					elsif (tratare_intrerupere(5) = '1') then
--						next_incheiere_tratare_intrerupere(5) <= '1';
--						next_validare_intrerupere(5) <= '0';
--						next_instructiune <= achitare_intrerupere_5;
--					elsif (tratare_intrerupere(6) = '1') then
--						next_incheiere_tratare_intrerupere(6) <= '1';
--						next_validare_intrerupere(6) <= '0';
--						next_instructiune <= achitare_intrerupere_6;
-- ============ se decomenteaza cind introduc intreruperi ===================
					end if;
					next_state <= execut_op;
				end if;
				
        when citesc_mem =>
				wea <= "0";
				adresa_ram <= contor_program;				
				case (instructiune) is
					when movla =>
						next_acumulator(7 downto 0) <= dout_ram;
						next_state <= extrag_op;
						next_contor_program <= contor_program + 1;
					when addla =>
						next_acumulator <= acumulator + dout_ram;
						next_contor_program <= contor_program + 1;
						next_state <= execut_op;
					when subla =>
						next_acumulator <= acumulator - dout_ram;
						next_contor_program <= contor_program + 1;
						next_state <= execut_op;
					when jmp =>
						next_contor_program <= dout_ram;
						next_state <= extrag_op;
					when movaf =>
						next_temp_contor_program <= contor_program + 1;
						next_contor_program <= dout_ram;
						next_state <= scriu_mem;
					when savecz =>
						next_temp_contor_program <= contor_program + 1;
						next_contor_program <= dout_ram;
						next_temp_acumulator(7) <= cf;
						next_temp_acumulator(6) <= zf;
						next_state <= scriu_mem;
					when movfa =>
						next_temp_contor_program <= contor_program + 1;
						next_contor_program <= dout_ram;
						next_state <= execut_op;
					when restcz =>
						next_temp_contor_program <= contor_program + 1;
						next_contor_program <= dout_ram;
						next_state <= execut_op;
					when addfa =>
						next_temp_contor_program <= contor_program + 1;
						next_contor_program <= dout_ram;
						next_state <= execut_op;
					when subfa =>
						next_temp_contor_program <= contor_program + 1;
						next_contor_program <= dout_ram;
						next_state <= execut_op;
					when incf =>
						next_temp_contor_program <= contor_program + 1;
						next_contor_program <= dout_ram;
						next_state <= execut_op;					
					when decf =>
						next_temp_contor_program <= contor_program + 1;
						next_contor_program <= dout_ram;
						next_state <= execut_op;					
					when call =>
						next_temp_acumulator(7 downto 0) <= contor_program + 1; -- adresa pentru stiva
						next_temp_contor_program <= dout_ram; -- adresa subprogramului
						next_contor_program <= contor_stiva; -- adresa unde scriu in stiva
						next_state <= scriu_mem;
					when ret =>
						next_contor_program <= dout_ram;
						next_state <= extrag_op;
					when outa =>
						next_numar_port_iesire <= dout_ram;
						next_contor_program <= contor_program + 1;
						next_state <= execut_op;
					when ina =>
						next_numar_port_intrare <= dout_ram;
						next_contor_program <= contor_program + 1;
						next_state <= execut_op;
					when andla =>
						next_acumulator(7 downto 0) <= acumulator(7 downto 0) and dout_ram;
						next_contor_program <= contor_program + 1;
						next_state <= execut_op;
					when orla =>
						next_acumulator(7 downto 0) <= acumulator(7 downto 0) or dout_ram;
						next_contor_program <= contor_program + 1;
						next_state <= execut_op;
					when andfa =>
						next_temp_contor_program <= contor_program + 1;
						next_contor_program <= dout_ram;
						next_state <= execut_op;					
					when orfa =>
						next_temp_contor_program <= contor_program + 1;
						next_contor_program <= dout_ram;
						next_state <= execut_op;					
					when xorla =>
						next_acumulator(7 downto 0) <= acumulator(7 downto 0) xor dout_ram;
						next_contor_program <= contor_program + 1;
						next_state <= execut_op;
					when xorfa =>
						next_temp_contor_program <= contor_program + 1;
						next_contor_program <= dout_ram;
						next_state <= execut_op;
					when page =>
						next_pagina <= dout_ram(4 downto 0);
						next_contor_program <= (others => '0');
						next_contor_stiva <= (others => '1');
						next_validare_intrerupere <= (others => '0');
						next_incheiere_tratare_intrerupere <=  (others => '0');
						next_state <= extrag_op;
					when movcaf =>
						next_temp_contor_program <= contor_program + 1;
						next_temp_pagina <= pagina;
						next_contor_program <= dout_ram;
						next_pagina <= pagina_selectata_transfer;
						next_state <= scriu_mem;
					when movcfa =>
						next_temp_contor_program <= contor_program + 1;
						next_temp_pagina <= pagina;
						next_contor_program <= dout_ram;
						next_pagina <= pagina_selectata_transfer;
						next_state <= execut_op;
					when others =>
						next_state <= extrag_op;
				end case;

         when scriu_mem =>
				case (instructiune) is
					when movaf =>
						adresa_ram <= contor_program;
						wea <= "1";
						next_contor_program <= temp_contor_program;
						din_ram <= acumulator(7 downto 0);
						next_state <= extrag_op;
					when savecz =>
						adresa_ram <= contor_program;
						wea <= "1";
						next_contor_program <= temp_contor_program;
						din_ram <= temp_acumulator(7 downto 0);
						next_state <= extrag_op;
					when incf =>
						adresa_ram <= contor_program;
						wea <= "1";
						next_contor_program <= temp_contor_program;
						din_ram <= acumulator(7 downto 0);
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_acumulator(7 downto 0) <= temp_acumulator(7 downto 0);
						next_acumulator(8) <= acumulator(8);
						next_state <= extrag_op;				
					when decf =>
						adresa_ram <= contor_program;
						wea <= "1";
						next_contor_program <= temp_contor_program;
						din_ram <= acumulator(7 downto 0);
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_acumulator(7 downto 0) <= temp_acumulator(7 downto 0);
						next_acumulator(8) <= acumulator(8);
						next_state <= extrag_op;				
					when call =>
						adresa_ram <= contor_program;
						wea <= "1";
						next_contor_program <= temp_contor_program;
						din_ram <= temp_acumulator(7 downto 0);
						next_contor_stiva <= contor_stiva - 1;
						next_state <= extrag_op;
					when movcaf =>
						adresa_ram <= contor_program;
						wea <= "1";
						next_contor_program <= temp_contor_program;
						din_ram <= acumulator(7 downto 0);
						next_pagina <= temp_pagina;
						next_state <= extrag_op;
					when others =>
						next_state <= extrag_op;
					end case;

			when execut_op =>
				case (instructiune) is
					when addla =>
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_state <= extrag_op;
					when subla =>
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_state <= extrag_op;
					when movfa =>
						wea <= "0";
						adresa_ram <= contor_program;
						next_contor_program <= temp_contor_program;
						next_acumulator(7 downto 0) <= dout_ram;
						next_state <= extrag_op;
					when restcz =>
						wea <= "0";
						adresa_ram <= contor_program;
						next_contor_program <= temp_contor_program;
						next_zf <= dout_ram(6);
						next_acumulator(8) <= dout_ram(7);
						next_state <= extrag_op;
					when addfa =>
						wea <= "0";
						adresa_ram <= contor_program;
						next_contor_program <= temp_contor_program;
						next_acumulator <= acumulator + dout_ram;
						next_state <= execut_op1;
					when subfa =>
						wea <= "0";
						adresa_ram <= contor_program;
						next_contor_program <= temp_contor_program;
						next_acumulator <= acumulator - dout_ram;
						next_state <= execut_op1;
					when incf =>
						wea <= "0";
						adresa_ram <= contor_program;
						next_acumulator <= (cf & dout_ram) + 1;
						next_state <= scriu_mem;				
					when decf =>
						wea <= "0";
						adresa_ram <= contor_program;
						next_acumulator <= (cf & dout_ram) - 1;
						next_state <= scriu_mem;
					when andla =>
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_state <= extrag_op;
					when orla =>
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_state <= extrag_op;					
					when andfa =>
						wea <= "0";
						adresa_ram <= contor_program;
						next_contor_program <= temp_contor_program;
						next_acumulator(7 downto 0) <= acumulator(7 downto 0) and dout_ram;
						next_state <= execut_op1;					
					when orfa =>
						wea <= "0";
						adresa_ram <= contor_program;
						next_contor_program <= temp_contor_program;
						next_acumulator(7 downto 0) <= acumulator(7 downto 0) or dout_ram;
						next_state <= execut_op1;					
					when xorla =>
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_state <= extrag_op;					
					when xorfa =>
						wea <= "0";
						adresa_ram <= contor_program;
						next_contor_program(7 downto 0) <= temp_contor_program;
						next_acumulator(7 downto 0) <= acumulator(7 downto 0) xor dout_ram;
						next_state <= execut_op1;
					when nega =>
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_state <= extrag_op;															
					when rlca =>
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_state <= extrag_op;										
					when rrca =>
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_state <= extrag_op;
					when ina =>
						if (numar_port_intrare = "11111111") then
							next_acumulator(7 downto 0) <= validare_intrerupere;
							next_state <= extrag_op;
						--elsif numar_port_intrare = "00000010" then -- stare interfata seriala
						--	next_acumulator(8) <= rx_empty;
						--	next_state <= extrag_op;							
						elsif numar_port_intrare = "00000110" then
							-- asta e facut pentru ca semnalul "citesc_serial"
							-- sa fie "1" doar pe durata unei perioade de tact
							next_acumulator(8) <= rx_empty;
							if rx_empty = '0' then
								next_rd_uart <= '1';
								next_acumulator(7 downto 0) <= r_data;
								next_state <= execut_op1;
							else
								next_state <= extrag_op;
							end if;
						else
							next_acumulator(7 downto 0) <= port_intrare;
							next_state <= extrag_op;
						end if;
					when outa =>
						if (numar_port_iesire = "11111111") then -- validare intreruperi
							next_port_iesire <= (others => '0');
							next_validare_intrerupere <= acumulator(7 downto 0);
							next_state <= extrag_op;
						end if;
						if (numar_port_iesire = "00000110") then -- scriere interfata seriala
							if tx_full = '0' then
								next_wr_uart <= '1';
								next_w_data <= acumulator(7 downto 0);
								next_state <= execut_op1;
							else
								--next_state <= extrag_op; --abandonez ?!
								next_state <= execut_op; -- repet pina scriu in uart								
							end if;
						end if;
						if (numar_port_iesire = "00000111") then -- trimitere pagina selectata pentru intreruperi
							next_port_iesire <= (others => '0');
							next_pagina_selectata_transfer <= acumulator(4 downto 0);
							next_state <= extrag_op;
						end if;
						if((numar_port_iesire /= "11111111") and 
						(numar_port_iesire /= "00000110") and
						(numar_port_iesire /= "00000111")) then
							next_port_iesire <= acumulator(7 downto 0);
							next_state <= extrag_op;
						end if;
					when movcfa =>
						wea <= "0";
						adresa_ram <= contor_program;
						next_contor_program <= temp_contor_program;
						next_acumulator(7 downto 0) <= dout_ram;
						next_pagina <= temp_pagina;
						next_state <= extrag_op;
					when achitare_intrerupere_0 =>
						next_incheiere_tratare_intrerupere(0) <= '0'; -- tratare intrerupere a fost sters si permit inregistrarea unei noi intreruperi cind intreruperea va fi activata
						next_temp_acumulator(7 downto 0) <= contor_program; -- adresa pentru stiva
						next_temp_contor_program <= "00000010"; -- adresa de tratare a intreruperii = 2
						next_contor_program <= contor_stiva; -- adresa unde scriu in stiva
						next_instructiune <= call;
						next_state <= scriu_mem;
					when achitare_intrerupere_1 =>
						next_incheiere_tratare_intrerupere(1) <= '0';
						next_temp_acumulator(7 downto 0) <= contor_program; -- adresa pentru stiva
						next_temp_contor_program <= "00000100"; -- adresa de tratare a intreruperii = 4
						next_contor_program <= contor_stiva; -- adresa unde scriu in stiva
						next_instructiune <= call;
						next_state <= scriu_mem;
-- ============ se decomenteaza cind introduc intreruperi ===================
--					when achitare_intrerupere_2 =>
--						next_incheiere_tratare_intrerupere(2) <= '0';
--						next_temp_acumulator(7 downto 0) <= contor_program; -- adresa pentru stiva
--						next_temp_contor_program <= "00000110"; -- adresa de tratare a intreruperii = 6
--						next_contor_program <= contor_stiva; -- adresa unde scriu in stiva
--						next_instructiune <= call;
--						next_state <= scriu_mem;
--					when achitare_intrerupere_3 =>
--						next_incheiere_tratare_intrerupere(3) <= '0';
--						next_temp_acumulator(7 downto 0) <= contor_program; -- adresa pentru stiva
--						next_temp_contor_program <= "00001000"; -- adresa de tratare a intreruperii = 8
--						next_contor_program <= contor_stiva; -- adresa unde scriu in stiva
--						next_instructiune <= call;
--						next_state <= scriu_mem;
--					when achitare_intrerupere_4 =>
--						next_incheiere_tratare_intrerupere(4) <= '0';
--						next_temp_acumulator(7 downto 0) <= contor_program; -- adresa pentru stiva
--						next_temp_contor_program <= "00001010"; -- adresa de tratare a intreruperii = 10
--						next_contor_program <= contor_stiva; -- adresa unde scriu in stiva
--						next_instructiune <= call;
--						next_state <= scriu_mem;
--					when achitare_intrerupere_5 =>
--						next_incheiere_tratare_intrerupere(5) <= '0';
--						next_temp_acumulator(7 downto 0) <= contor_program; -- adresa pentru stiva
--						next_temp_contor_program <= "00001100"; -- adresa de tratare a intreruperii = 12
--						next_contor_program <= contor_stiva; -- adresa unde scriu in stiva
--						next_instructiune <= call;
--						next_state <= scriu_mem;
--					when achitare_intrerupere_6 =>
--						next_incheiere_tratare_intrerupere(6) <= '0';
--						next_temp_acumulator(7 downto 0) <= contor_program; -- adresa pentru stiva
--						next_temp_contor_program <= "00001110"; -- adresa de tratare a intreruperii = 14
--						next_contor_program <= contor_stiva; -- adresa unde scriu in stiva
--						next_instructiune <= call;
--						next_state <= scriu_mem;
-- ============ se decomenteaza cind introduc intreruperi ===================
					when others =>
						next_state <= extrag_op;
				end case;

			when execut_op1 =>				
				case (instructiune) is
					when addfa =>
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_state <= extrag_op;						
					when subfa =>
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_state <= extrag_op;
					when andfa =>
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_state <= extrag_op;											
					when orfa =>
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_state <= extrag_op;											
					when xorfa =>
						next_zf <= not(acumulator(7) or acumulator(6) or acumulator(5) or acumulator(4) or acumulator(3) or acumulator(2) or acumulator(1) or acumulator(0));
						next_state <= extrag_op;
					when outa => -- incheiere scriere in interfata seriala
						next_wr_uart <= '0';
						-- next_numar_port_iesire <= "00000000";
						next_state <= extrag_op;
					when ina => -- incheiere citire din interfata seriala
						next_rd_uart <= '0';
						next_state <= extrag_op;
					when others =>
						next_state <= extrag_op;
				end case;
         when others =>
            next_state <= extrag_op;
      end case;      
   end process;

reset <= btn_reset;
--reset <= '0';
led <= led_i;

-- DEFINIREA INTRARILOR
-- =====================
intrare: process(btn, sw, next_numar_port_intrare, contor_timer,
port_intrare)
begin
	next_port_intrare <= port_intrare; -- pentru evitare latches
	if (next_numar_port_intrare = "00000000") then -- port 0 citeste starea butoanelor de pe placa
		next_port_intrare(3 downto 0) <= btn(3 downto 0);
	elsif (next_numar_port_intrare = "00000001") then -- port 1 citeste starea switch-urilor de pe placa
		next_port_intrare <= sw;
	elsif (next_numar_port_intrare = "00000010") then -- port 2 citeste starea intrarilor port JA
		next_port_intrare <= port_intrare1;
	elsif (next_numar_port_intrare = "00000101") then -- port 5 citeste contor timer
		next_port_intrare <= contor_timer;
		-- port 6 rezervat pentru UART
	elsif (next_numar_port_intrare = "00001001") then -- port 9 citeste starea intrarilor port JC
		next_port_intrare <= port_in_out1;
		-- port 255 rezervat pentru validare intrerupere
	else
		next_port_intrare <= "00000000";
	end if;

end process;
-- ====================================================

adresa_extinsa <= pagina & adresa_ram;

-- ====================================================
-- Intreruperi
-- ====================================================

	int0 <= tick_timer; --trecere prin zero a timer1
	int1 <= not rx_empty; -- data_serial_receptionata

-- folosite
--int0 <= '0';
--int1 <= '0';
-- nefolosite
--int2 <= '0';
--int3 <= '0';
--int4 <= '0';
--int5 <= '0';
--int6 <= '0';


end Behavioral;

