----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 12.10.2021 09:15:45
-- Design Name: 
-- Module Name: decodor_TB - Behavioral
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
-- pentru a converti tipul si a incrementa vectorul

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity decodor_TB is
--  Port ( );
end decodor_TB;

-- Varianta Testbench pentru VHDL 87 care este modul de instantiere

architecture Behavioral of decodor_TB is

-- declararea componentei
component decodor
	port(
	intrare: in std_logic_vector(3 downto 0);
	iesire: out std_logic_vector(9 downto 0)
	);
end component;

signal test_intrare: std_logic_vector(3 downto 0);
signal test_iesire: std_logic_vector(9 downto 0);

begin

-- instantierea circuitului de testat uut
uut: decodor
	port map(intrare => test_intrare, iesire => test_iesire);

-- generarea vectorilor de test
	process
	begin
		test_intrare <= "0000";
		wait for 100 ns;
--	bucla infinita
	loop
		test_intrare <= std_logic_vector(unsigned(test_intrare) + "1");
		wait for 100 ns;
	end loop;
	end process;

end Behavioral;
