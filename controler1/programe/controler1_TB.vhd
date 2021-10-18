--------------------------------------------------------------------------------
-- Company: 
-- Engineer:
--
-- Create Date:   07:49:06 10/18/2021
-- Design Name:   
-- Module Name:   C:/LUCRU/controler1/surse/controler1_TB.vhd
-- Project Name:  controler1
-- Target Device:  
-- Tool versions:  
-- Description:   
-- 
-- VHDL Test Bench Created by ISE for module: controler1
-- 
-- Dependencies:
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
--
-- Notes: 
-- This testbench has been automatically generated using types std_logic and
-- std_logic_vector for the ports of the unit under test.  Xilinx recommends
-- that these types always be used for the top-level I/O of a design in order
-- to guarantee that the testbench will bind correctly to the post-implementation 
-- simulation model.
--------------------------------------------------------------------------------
LIBRARY ieee;
USE ieee.std_logic_1164.ALL;
 
-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--USE ieee.numeric_std.ALL;
 
ENTITY controler1_TB IS
END controler1_TB;
 
ARCHITECTURE behavior OF controler1_TB IS 
 
    -- Component Declaration for the Unit Under Test (UUT)
 
    COMPONENT controler1
    PORT(
         clk : IN  std_logic;
         reset : IN  std_logic;
         led : OUT  std_logic_vector(7 downto 0)
        );
    END COMPONENT;
    

   --Inputs
   signal clk : std_logic := '0';
   signal reset : std_logic := '0';
--	signal data_mem : std_logic_vector(7 downto 0);

 	--Outputs
   signal led : std_logic_vector(7 downto 0);
	
    -- Clock period definitions
   constant T : time := 100 ns; -- perioada de ceas 
BEGIN
 
	-- Instantiate the Unit Under Test (UUT)
   uut: controler1 PORT MAP (
          clk => clk,
          reset => reset,
          led => led
        );

   -- Clock process definitions
   clk_process :process
   begin
		clk <= '0';
		wait for T/2;
		clk <= '1';
		wait for T/2;
   end process;
 
-- reset activat pentru T/2
      reset <= '1', '0' after T/2;

   -- Stimulus process

END;
