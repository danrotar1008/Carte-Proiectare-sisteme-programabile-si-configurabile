----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 10.10.2021 17:59:10
-- Design Name: 
-- Module Name: mux8x1_TB - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
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

use ieee.numeric_std.all;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity mux8x1_TB is
--  Port ( );
end mux8x1_TB;

architecture Behavioral of mux8x1_TB is

    -- Component Declaration for the Unit Under Test (UUT)
 
    COMPONENT mux8x1
    Port ( in_mux : in STD_LOGIC_VECTOR (7 downto 0);
           in_comanda : in STD_LOGIC_VECTOR (2 downto 0);
           ies_mux : out STD_LOGIC);
    END COMPONENT;
 
signal test_intrare: std_logic_vector(7 downto 0);
signal test_comanda: std_logic_vector(2 downto 0);
signal test_iesire: std_logic;

begin
uut: mux8x1
	port map(in_mux => test_intrare, in_comanda => test_comanda, ies_mux => test_iesire);

-- generarea vectorilor de test
	process
	variable contor: integer;
	begin
		test_intrare <= "00000000";
		test_comanda <= "000";
		wait for 100 ns;
		loop -- bucla infinita
			for contor in 0 to 7 loop
				test_intrare(contor) <= '1';
				wait for 100 ns;
				test_intrare <= "00000000";
			end loop;
			test_comanda <= std_logic_vector(unsigned(test_comanda) + "1");
			wait for 100 ns;
		end loop;
	end process;

end Behavioral;
