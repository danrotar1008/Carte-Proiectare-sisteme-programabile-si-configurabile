----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    09:47:09 10/14/2021 
-- Design Name: 
-- Module Name:    fsm1 - Behavioral 
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

entity fsm1 is
    Port ( clk : in  STD_LOGIC;
           reset : in  STD_LOGIC;
           sw : in  STD_LOGIC_VECTOR (7 downto 0);
           led : out  STD_LOGIC_VECTOR (7 downto 0));
end fsm1;

architecture Behavioral of fsm1 is

   --Utilizați nume descriptive pentru stări, cum ar fi st1_reset, st2_search
    type state_type is (st1_start, st2_1, st3_2, st4_3); 
    signal state, next_state : state_type;  
   --Declarați semnale interne pentru toate ieșirile mașinii de stare
	 signal led_i : std_logic_vector(7 downto 0);
   --alte ieșiri

begin

   SYNC_PROC: process (clk)
   begin
      if (clk'event and clk = '1') then
         if (reset = '1') then
            state <= st1_start;
            led <= "00000000";
         else
            state <= next_state;
            led <= led_i;
         end if;        
      end if;
   end process;
 
   --Mașina cu stări MOORE - Ieșiri bazate numai pe stare
   OUTPUT_DECODE: process (state)
   begin
      --introduceți instrucțiuni pentru a decoda semnalele de ieșire interne
      if state = st1_start then
         led_i <= "10000001";
      end if;
      if state = st2_1 then
         led_i <= "11110000";
      end if;
      if state = st3_2 then
         led_i <= "11001100";
      end if;
      if state = st4_3 then
         led_i <= "10101010";
      end if;
   end process;
 
   NEXT_STATE_DECODE: process (state, sw)
   begin
      --declarați starea implicită pentru next_state pentru a evita blocările 
		next_state <= state; --implicit este să rămâi în starea actuală
      --inserează instrucțiuni pentru a decoda next_state
      case (state) is
         when st1_start =>
            if sw = "00000001" then
               next_state <= st2_1;
				else
					next_state <= st1_start;
            end if;
         when st2_1 =>
            if sw = "00000010" then
               next_state <= st3_2;
				elsif sw = "00000011" then
					next_state <= st4_3;
				else
					next_state <= st2_1;
            end if;
         when st3_2 =>
				if sw = "00000100" then
					next_state <= st1_start;
				else
					next_state <= st3_2;
				end if;
         when st4_3 =>
				if sw = "00000100" then
					next_state <= st1_start;
				else
					next_state <= st4_3;
				end if;
         when others =>
            next_state <= st1_start;
      end case;      
   end process;

end Behavioral;

