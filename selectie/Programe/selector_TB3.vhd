----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 12.10.2021 14:42:37
-- Design Name: 
-- Module Name: selector_TB - Behavioral
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

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity selector_TB is
--  Port ( );
end selector_TB;

architecture Behavioral of selector_TB is

-- declararea componentei
component selector
	port(
	intrare: in std_logic_vector(3 downto 0);
	iesire: out std_logic
	);
end component;

signal test_intrare: std_logic_vector(3 downto 0);
signal test_iesire: std_logic;

begin
-- instantierea circuitului de testat uut
uut: selector
	port map(intrare => test_intrare, iesire => test_iesire);

-- generarea vectorilor de test
	process
	begin
		-- test_intrare <= "0000";
		-- varianta
		test_intrare <= std_logic_vector(to_unsigned (0, 4));
		
		wait for 100 ns;
--	bucla infinita
	loop
		test_intrare <= std_logic_vector(unsigned(test_intrare) + "1");
		wait for 100 ns;
	end loop;
	end process;

end Behavioral;
