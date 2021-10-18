----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    07:25:04 10/18/2021 
-- Design Name: 
-- Module Name:    controler1 - Behavioral 
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

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity controler1 is
    Port ( clk : in  STD_LOGIC;
           reset : in  STD_LOGIC;
           led : out  STD_LOGIC_VECTOR (7 downto 0));
end controler1;

architecture Behavioral of controler1 is

	COMPONENT contor1
		PORT (
			clk : IN STD_LOGIC;
			q : OUT STD_LOGIC_VECTOR(33 DOWNTO 0)
		);
	END COMPONENT;

	COMPONENT mem1
		PORT (
			clka : IN STD_LOGIC;
			addra : IN STD_LOGIC_VECTOR(7 DOWNTO 0);
			douta : OUT STD_LOGIC_VECTOR(7 DOWNTO 0)
		);
	END COMPONENT;

   --Utiliza?i nume descriptive pentru stãri, cum ar fi st1_reset, st2_search
    type state_type is (st1_start, st1, st2); 
    signal state, next_state : state_type;  
   --Declara?i semnale interne pentru toate ie?irile ma?inii de stare
	 signal led_i : std_logic_vector(7 downto 0);
   --alte ie?iri
	 signal data_mem : std_logic_vector(7 downto 0);
	 signal q : STD_LOGIC_VECTOR(33 DOWNTO 0);
	 
begin

		contor_adresa : contor1
		PORT MAP (
			clk => clk,
			q => q
		);

		memorie_date: mem1
		PORT MAP (
			clka => clk,
			-- addra => q(33 downto 26),
			addra => q(7 downto 0), -- pentru simulare
			douta => data_mem
		); 

   SYNC_PROC: process (clk)
   begin
      if (clk'event and clk = '1') then
         if (reset = '1') then
            state <= st1_start;
            led <= "10101010";
         else
            state <= next_state;
            led <= led_i;
         end if;        
      end if;
   end process;
 
   --Ma?ina cu stãri MOORE - Ie?iri bazate numai pe stare
   OUTPUT_DECODE: process (state)
   begin
      --introduce?i instruc?iuni pentru a decoda semnalele de ie?ire interne
      if state = st1_start then
         led_i <= "10101010";
      end if;
      if state = st2 then
         led_i <= "11110000";
      end if;
      if state = st1 then
         led_i <= "10101010";
      end if;
   end process;
 
   NEXT_STATE_DECODE: process (state, data_mem)
   begin
      --declara?i starea implicitã pentru next_state pentru a evita blocãrile 
		next_state <= state; --implicit este sã se ramânã în starea actualã
      --insera?i instruc?iuni pentru a decoda next_state
      case (state) is
         when st1_start =>
            if data_mem(0) = '1' then -- data impara
               next_state <= st2;
				else
					next_state <= st1_start;
            end if;
         when st2 =>
            if data_mem(0) = '0' then -- data parã
               next_state <= st1;
				else
					next_state <= st2;
            end if;
         when st1 =>
	     if data_mem(0) = '1' then -- data_impara
		next_state <= st2;
				else
					next_state <= st1;
				end if;
         when others =>
            next_state <= st1_start;
      end case;      
   end process;

end Behavioral;

