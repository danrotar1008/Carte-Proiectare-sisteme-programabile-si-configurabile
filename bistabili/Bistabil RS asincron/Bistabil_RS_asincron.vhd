----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 12.10.2021 17:37:04
-- Design Name: 
-- Module Name: Bistabil_RS_asincron - Behavioral
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

entity Bistabil_RS_asincron is
    Port ( in_RS : in STD_LOGIC_VECTOR (1 downto 0);
           Q : out STD_LOGIC;   -- iesire Q a bistabilului
           nQ : out STD_LOGIC); -- iesire Q negat a bistabilului
end Bistabil_RS_asincron;

architecture Behavioral of Bistabil_RS_asincron is

-- Q_pres = starea prezenta
-- acest semnal s-a introdus pentru nQ, altfel nu e necesar
signal Q_pres: std_logic;
begin
	process(in_RS)
	begin
		if in_RS = "10" then Q_pres <= '0';
		elsif in_RS = "01" then Q_pres <= '1';
		elsif in_RS = "11" then Q_pres <='U';
		end if;
	end process;
		Q <= Q_pres;
		nQ <= not Q_pres;

end Behavioral;
