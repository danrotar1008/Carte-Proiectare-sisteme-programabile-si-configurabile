----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 12.10.2021 17:44:29
-- Design Name: 
-- Module Name: Bistabil_RSasinc_TB - Behavioral
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
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity Bistabil_RSasinc_TB is
--  Port ( );
end Bistabil_RSasinc_TB;

architecture Behavioral of Bistabil_RSasinc_TB is

component Bistabil_RS_asincron
	port(
        in_RS : in STD_LOGIC_VECTOR (1 downto 0);
        Q : out STD_LOGIC;   -- iesire Q a bistabilului
        nQ : out STD_LOGIC -- iesire Q negat a bistabilului
	);
end component;

signal in_RS_test: std_logic_vector (1 downto 0);
signal Q_test: std_logic;

begin
uut: Bistabil_RS_asincron
	port map(in_RS => in_RS_test, Q => Q_test);
	
	process
	begin
		in_RS_test <= "11";
		wait for 100 ns;	
		in_RS_test <= "01";
		wait for 100 ns;	
		in_RS_test <= "10";
		wait for 100 ns;	
		in_RS_test <= "00";
		wait for 100 ns;	

	end process;

end Behavioral;
